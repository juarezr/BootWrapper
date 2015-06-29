/// Classe responsável em conter os programas java Scripts que interagem com os objetos Helpers do MVC
/// Qualquer customização para CRUD's especificos deve ser tratado em um JS a parte
/// JavaScript Design Patterns (Prototype Pattern) - ver Learning JavaScript Design Patterns A book by Addy Osmani Volume 1.5.2
function BootWrapperHelpers() {
        
    this.IdFormulario = "";
    this.urlValidacao = "";
};

/// Função que ocorre no evento change do controle
BootWrapperHelpers.prototype.ValidarModel = function ValidarModel(obj) {

    //Remove mensagem se ainda estiver no client
    $(obj).popover('destroy');
   

    //Limpa os controles apontados com erro
    $('.form-group.has-error').each(function () {
        $(this).removeClass('form-group has-error');
        $(this).addClass('form-group');
    });

    
    //Serializa o formulário para enviar como Model
    var dados = $('#' + this.IdFormulario).serialize();
    
    //Realiza a chamada Ajax como GET e solicita a validação dos campos
    $.ajax({
        url: this.urlValidacao,
        data: JSON.stringify(dados),
        contentType: 'application/json',
        type: 'GET',
        success: function (response) {

            //Percorre a resposta atraz de mensagens de erros sobre os controles da Model
            $.each(response, function () {
                var idControle = '#' + this['IdControle'].replace('lbl_', '');

                //Configura o popover do BootStrap
                $(idControle).popover(
                                    {
                                        html: true,
                                        placement: 'top',
                                        content: this['MensagemValidacao']
                                    });
                //Excecuta o popover
                $(idControle).popover('show');

                if ($(idControle).is(':checkbox') == false && $(idControle).is(':radio') == false) {

                    $(idControle).parents('div').each(function () {
                        if ($(this).hasClass('form-group')) {
                            $(this).removeClass('form-group');
                            $(this).addClass('form-group  has-error');
                            return;
                        }
                    });

                }
            });
        }
    });

    primavera.BWHelpers.AmendBooleanFields('#' + this.IdFormulario);
};



///Função de ajuste para tratar campos bool (checkbox e radio's) Bug do Jquery que seraliza esse tipo com valor incorreto
BootWrapperHelpers.prototype.AmendBooleanFields = function (idForm) {
    var checkboxes = $(idForm).find('input[type="checkbox"]');
    $.each(checkboxes, function () {
        $(this).popover('hide');
    });

    var radios = $(idForm).find('input[type="radio"]');
    $.each(radios, function () {
        $(this).popover('hide');
    });
};


BootWrapperHelpers.prototype.ShowErrorMessage = function (message) {
    var mytheme = 'future';
    var mypos = 'messenger-on-bottom messenger-on-right';

    //Set theme
    Messenger.options = {
        extraClasses: 'messenger-fixed ' + mypos,
        theme: mytheme
    }
    Messenger().run({
        errorMessage: message,
        showCloseButton: true,
        action: function (opts) { return opts.error(); }
    });
};

BootWrapperHelpers.prototype.CloseMessage = function () {
        $('#divMensagemValidacao').fadeOut(2600);
};

BootWrapperHelpers.prototype.CloseConfirm = function () {
        $('#ModalConfirm').modal('hide');
    };

BootWrapperHelpers.prototype.OpenConfirm = function (func) {

        $('#confirmFunction').val(func);
        $('#ModalConfirm').modal('show');
};

BootWrapperHelpers.prototype.CleanBody = function () {

    if ($('body').is('.modal-open')) {
        $('body').removeClass('modal-open');
    }
    $('.modal-backdrop').hide();

};

BootWrapperHelpers.prototype.ExecFnConfirm = function () {
    var execute = $('#confirmFunction').val();
    if (execute != "") {
        var fn = new Function(execute);
        fn();
    }
};


BootWrapperHelpers.prototype.OpenLoading = function AbrirLoading() {
    $("#page").css({ opacity: 0.50 });
    $('#loading-indicator').show();

       
};

BootWrapperHelpers.prototype.CloseLoading = function FecharLoading() {
    $("#page").css({ opacity: 100.0 });
    $('#loading-indicator').hide();
};
   
BootWrapperFW.prototype.bwHelpers = new BootWrapperHelpers();
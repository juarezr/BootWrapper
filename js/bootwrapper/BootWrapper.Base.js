/** 
 * Main class of Framework BootWrapper.  
 * 
 * @version 1.0
 *  
 */
function BootWrapper() {
    
    this.Strings = Object.create(BootWrapper.Strings);
    this.Types = Object.create(BootWrapper.Types);

    this.ajaxGenericError = function (jqXhr, textStatus, errorThrown) {
        alert('$.post error: ' + textStatus + ' : ' + errorThrown);        
        window.bootwrapper.base.CloseLoading();
        window.bootwrapper.base.ExibirMensagemErro("Ocorreu um erro inesperado na chamada ao servidor.");
    };    
};

/**
 * Função de log no console.
 */
BootWrapper.prototype.Log = function (msg) {
    if (typeof console != "undefined") {
        console.log(msg);
    }
};

/**
 * Verifica se variável existe e está definida. Ou seja, se for diferente de null e diferente de 'undefined' retorna true, false caso contrário.
 * 
 * @param {string} obj  - Objeto a ser inspecionado.
 */
BootWrapper.prototype.IsVarDefined = function (obj) {
    if (obj === null || obj === undefined) {
        return false;
    }
    
    return true;
};

BootWrapper.prototype.ClassOf = function (o) {
    if (o === null) {
        return "null";
    }
    if (o === undefined) {
        return "undefined";
    }
    return Object.prototype.toString.call(o).slice(8, -1).toLowerCase();
};



/**
 * Funções uteis definição de tipos.
 */
BootWrapper.Types =
{
    isDate: function (o) {
        return window.bootwrapper.base.ClassOf(o) === "date";
    },

    isFunction: function (o) {
        return window.bootwrapper.base.ClassOf(o) === "function";
    },

    isGuid: function (value) {
        return (typeof value === "string") && /[a-fA-F\d]{8}-(?:[a-fA-F\d]{4}-){3}[a-fA-F\d]{12}/.test(value);
    },

    isEmpty: function (obj) {
        if (obj === null || obj === undefined) {
            return true;
        }
        for (var key in obj) {
            if (Object.prototype.hasOwnProperty.call(obj, key)) {
                return false;
            }
        }
        return true;
    },

    isNumeric: function (n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    },

    isArray: function (obj) {
        return Object.prototype.toString.call(obj) === '[object Array]';
    }
};

/**
 * Funções uteis para strings.
 */
BootWrapper.Strings =
{
    startsWith: function (str, prefix) {
        // returns false for empty strings too
        if ((!str) || !prefix) return false;
        return str.indexOf(prefix, 0) === 0;
    },

    endsWith: function (str, suffix) {
        // returns false for empty strings too
        if ((!str) || !suffix) return false;
        return str.indexOf(suffix, str.length - suffix.length) !== -1;
    },

    // Based on fragment from Dean Edwards' Base 2 library
    // format("a %1 and a %2", "cat", "dog") -> "a cat and a dog"
    format: function (string) {
        var args = arguments;
        var pattern = RegExp("%([1-" + (arguments.length - 1) + "])", "g");
        return string.replace(pattern, function (match, index) {
            return args[index];
        });
    },

    formatObject: function (fieldPattern, jsonObject) {
        for (var prop in jsonObject) {
            var value = jsonObject[prop];
            fieldPattern = fieldPattern.replace('{' + prop + '}', value);
        }
        return fieldPattern;
    }
};

/**
 * Função para substituir mensagens no container indicado.
 * 
 * @param {string} placeholder  - Id do container onde as mensagens aparecerão.
 * @param {string} messageType  - Tipo da mensagem define a cor do placeholder.
 * @param {string} messageList  - Mensagens.
 */
BootWrapper.prototype.ReplaceMessages = function (placeholder, messageType, messageList) {
    if (messageList.length < 1) {
        return {};
    }
    var messageDiv = $('#' + messageType);
    messageDiv.remove();
    for (var message in messageList) {
        var msg = messageList[message];
        window.bootwrapper.bwValidation.AddMessage(placeholder, messageType, msg);
    }
    return messageDiv;
};

/**
 * Envia por ajax o form para o data-action indicado e coloca as mensagens do resultado no messageContainer informado.
 * 
 * @param {string} formId  - Id do form.
 * @param {string} messageContainer  - Container de mensagens.
 * @param {string} fnCallback  - função de processamento após a execução.
 */
BootWrapper.prototype.SaveForm = function (formId, messageContainer, fnCallback) {

    function handleReturn(data) {

        try {
            if (messageContainer) {
                window.bootwrapper.base.ReplaceMessages(messageContainer, BootWrapperValidation.MessageType.Error, data.Errors);
                window.bootwrapper.base.ReplaceMessages(messageContainer, BootWrapperValidation.MessageType.Warning, data.Warnings);
                window.bootwrapper.base.ReplaceMessages(messageContainer, BootWrapperValidation.MessageType.Success, data.Messages);
            }
        } finally {
            window.bootwrapper.base.CloseLoading();
            if (BootWrapper.Types.isFunction(fnCallback)) {
                fnCallback(data.Errors.length < 1, data.Entity);
            }
        }
    }

    this.Log(BootWrapper.Strings.format("Saving data from %1", formId));
    var form = $('#' + formId).first();
    
    // Verifica se há alguma validação incorreta com o JQuery.Validate
    if (form.attr("data-validate")) {
        if (!form.valid()) {
            return {};
        }
    }

    var dataAction = form.attr("data-action");
    if (BootWrapper.IsVarDefined(dataAction) && (dataAction.toLowerCase() == 'submit')) {
        form.submit();
    } else {
        var formAction = form.attr("action");
        if (!formAction) {
            alert('atributo action nao definido para form ' + formId);
            return {};
        }
        var formMethod = form.attr("method");
        formMethod = formMethod ? formMethod.toUpperCase() : 'POST';

        window.bootwrapper.base.OpenLoading();
                
        if (formMethod == 'GET') {
            $.get(formAction, form.serialize(), handleReturn, "json").fail(this.ajaxGenericError);
        } else {
            $.post(formAction, form.serialize(), handleReturn, "json").fail(this.ajaxGenericError);
        }
    }
    return form;
}

/**
 * Valida o formulário e chama o método submit.
 * @param {string} formId  - Id do form.
 */
BootWrapper.prototype.SubmitForm = function (formId) {
    this.Log(BootWrapper.Strings.format("Submitiing form %1", formId));
    var form = $('#' + formId).first();

    window.bootwrapper.base.OpenLoading();

    //Verifica se há alguma validação incorreta com o JQuery.Validate
    if (form.attr("data-validate")) {
        if (form.valid()) {
            form.submit();
        }
    }

    window.bootwrapper.base.CloseLoading();

    return form;
}

/**
* Envia por ajax o form para o data-action indicado e coloca as mensagens do resultado no messageContainer informado.
* 
* @param {string} formId  - Id do form.
* @param {string} messageContainer  - Container de mensagens.
* @param {string} fnCallback  - função de processamento após a execução. Parametros: 1-Sucesso true ou false, 2-JSON retornado pelo Ajax, 3-fnParam
* @param {string} fnParam  - parametro para ser passado de retorno para a função.
* @param {string} formMethod  - GET ou POST, default: atributo method do form (default POST)
*/
BootWrapper.prototype.ExecuteActionWithForm = function (formId, formAction, messageContainer, fnCallback, fnParam, formMethod) {

    function handleReturn(data) {

        try {
            if (messageContainer) {
                window.bootwrapper.base.ReplaceMessages(messageContainer, BootWrapperValidation.MessageType.Error, data.Errors);
                window.bootwrapper.base.ReplaceMessages(messageContainer, BootWrapperValidation.MessageType.Warning, data.Warnings);
                window.bootwrapper.base.ReplaceMessages(messageContainer, BootWrapperValidation.MessageType.Success, data.Messages);
            }
        } finally {
            window.bootwrapper.base.CloseLoading();
            if (BootWrapper.Types.isFunction(fnCallback)) {
                fnCallback(data.Errors.length < 1, data.Entity, fnParam);
            }
        }
    }

    this.Log(BootWrapper.Strings.format("Executing action with '%1' in '%2'", formId, formAction));
    var form = $('#' + formId).first();

    var formMethod2 = formMethod ? formMethod : form.attr("method");
    formMethod2 = formMethod2 ? formMethod2.toUpperCase() : 'POST';

    window.bootwrapper.base.OpenLoading();

    if (formMethod2 == 'GET') {
        $.get(formAction, form.serialize(), handleReturn, "json").fail(this.ajaxGenericError);
    } else {
        $.post(formAction, form.serialize(), handleReturn, "json").fail(this.ajaxGenericError);
    }

    return form;
}


/**
 * Executa uma action e coloca as mensagens do resultado no messageContainer indicado.
 * Essa função é útil para realizar chamadas rápidas no servidor sem necessidade de se obter uma resposta.
 * 
 *  @param {string} actionUrl  - URL contendo o controller e action para realizar o request.
 *  @param {json} jsonData     - JSon enviado para a action do controller.
 *  @param {string} messageContainer - Id do container que receberá as mensagens de execução.
 *  @param {string} fnCallback - Função que executará após o processamento. 
 *  @param {string} method - POST ou GET. Se não informado, POST será utilizado.
 */
BootWrapper.prototype.ExecuteAction = function (actionUrl, jsonData, messageContainer, fnCallback, method) {

    function handleReturn(data) {

        try {
            if (messageContainer) {
                window.bootwrapper.base.ReplaceMessages(messageContainer, BootWrapperValidation.MessageType.Error, data.Errors);
                window.bootwrapper.base.ReplaceMessages(messageContainer, BootWrapperValidation.MessageType.Warning, data.Warnings);
                window.bootwrapper.base.ReplaceMessages(messageContainer, BootWrapperValidation.MessageType.Success, data.Messages);
            }
        } finally {
            window.bootwrapper.base.CloseLoading();
            if (BootWrapper.Types.isFunction(fnCallback)) {
                fnCallback(data.Errors.length < 1, data.Entity);
            }
        }
    }

    method = method ? method : 'POST';

    window.bootwrapper.base.OpenLoading();

    if (method == 'GET') {
        $.get(actionUrl, jsonData, handleReturn, "json").fail(this.ajaxGenericError);
    } else {
        $.post(actionUrl, jsonData, handleReturn, "json").fail(this.ajaxGenericError);
    }
}

/**
 * Carrega uma action com os argumentos informados em json e o resultado é inserido como Html no container indicado.
 * Essa função é útil quando o controller retorna uma partial view.
 * 
 *  @param {string} actionUrl - URL contendo o controller e action para realizar o request.
 *  @param {json} jsonData    - JSon enviado para a action do controller.
 *  @param {string} container - Id do select onde o filtro será realizado.
 *  @param {string} fnCallback - Função que executará após o processamento. 
 */
BootWrapper.prototype.LoadAction = function (actionUrl, jsonData, container, fnCallback) {

    function handleReturn(data) {
        try {
            if (container) {
                $("#" + container).html(data);
            }
        } finally {
            if (BootWrapper.Types.isFunction(fnCallback)) {
                fnCallback();
            }
        }
    }

    // Do the ajax call
    $.ajax({
        url: actionUrl,
        type: 'POST',
        async: true,
        data: jsonData,
        success: function (data) {
            handleReturn(data);
        }
    });
}

/**
 * Filtra um dropdown list através de uma consulta no controller informardo por actionUrl, cujo retorno em formato Json deve ser uma lista contendo objetos que contenham as propriedades 'Value' e 'Text',
 * seguindo as mesmas regras do SelectListItem.
 * 
 *  @param {string} actionUrl - URL contendo o controller e action para realizar o request.
 *  @param {json} jsonData    - JSon enviado para a action do controller.
 *  @param {string} container - Id do select onde o filtro será realizado.
 *  @param {string} fnCallback - Função que executará após o processamento.
 */
BootWrapper.prototype.FilterDropdownAction = function (container, actionUrl, jsonData, fnCallback) {

    function handleReturn(data) {

        try {
            if (container) {
                var dropDownList = $('#' + container);

                window.bootwrapper.base.ClearDropdownAction(container);
                
                for (var i = 0; i < data.length; i++) {
                    var option = $('<option></option>').attr('value', data[i].Value).text(data[i].Text);
                    if (BootWrapper.IsVarDefined(data[i].Selected) && data[i].Selected)
                        option.attr('selected', 'true');

                    dropDownList.append(option);
                }
            }
        } finally {
            if (BootWrapper.Types.isFunction(fnCallback)) {
                fnCallback();
            }
        }
    }

    $.post(actionUrl, jsonData, handleReturn, "json").fail(this.ajaxGenericError);
};

/**
 * Limpa os campos do dropdown deixando apenas o campo selecione.
 * @param {string} container - Id do dropdown list.
 * 
 */
BootWrapper.prototype.ClearDropdownAction = function (container) {

    if (container) {
        var dropDownList = $('#' + container);
        dropDownList.empty();
        //dropDownList.find('option').remove();
        dropDownList.append($('<option></option>').attr('value', '').text('Selecione'));
    }
};


/**
 * Mostra uma seção div como modal.
 *  @param {string} divmodalId - Id da div.
 */
BootWrapper.prototype.ShowModal = function (divmodalId) {
    var options = { backdrop: true, keyboard: true, show: true }
    $('#' + divmodalId).modal(options);
    $('#' + divmodalId + ' input:visible').first().focus();
};


/**
 * Inicializa todos os tipos input que declaram a classe 'clockpicker' para seleção de hora.
 */
BootWrapper.prototype.InitClockPicker = function () {
    
    // Referencia: http://weareoutman.github.io/clockpicker/, atual
    $('.clockpicker').each(function (i, control) {

        var options = {
            donetext: "OK"
        };

        $(control).clockpicker(options);
    });

};

/**
 * Inicializa todos os inputs que possuem o data-control=datepicker para seleção de data.
 */
BootWrapper.prototype.InitDatePicker = function () {
   
    // Referencia: https://github.com/eternicode/bootstrap-datepicker            
    $('input[data-control=datepicker]').each(function (i, control) {

        var language = window.navigator.userLanguage || window.navigator.language;

        var formatField = $(this).attr("data-date-format");
        var minViewMode = $(this).attr("date-min-viewmode");
            minViewMode = minViewMode ? parseInt(minViewMode) : 0;

        var options = {
            language: language,
            format: formatField === undefined ? 'dd/mm/yyyy' : formatField,
            todayHighlight: true,
            forceParse: true,
            todayBtn: minViewMode == 0 ? true : false,
            clearBtn: true,
            showOn: 'both',
            minViewMode: minViewMode,
        };

        var datestart = $(control).attr('data-date-start');
        if (datestart) {
            options.startDate = datestart;
        }
        var dateend = $(control).attr('data-date-end');
        if (dateend) {
            options.endDate = dateend;
        }

        //Verifica se o controle está configurado para ser validado no momento de alteração
        //Se estiver implementa no evento fechar do datepicker a chamada ao métodod e validação
        //if ($(this).attr('Validar') == 'True') {
        //    $(this).datepicker()
        //    .on('hide', function (e) {
        //        bootwrapper.bwHelpers.ValidarModel(this);
        //    });
        //}

        $(control).datepicker(options);        

        // show datepicker on click on icon-time
        $(control).parent('.input-group, .input-prepend').find('.input-group-addon').on('click', function () {
            $(control).datepicker('show');
        });
    });

};

/**
 * Realizar o collapse de um painel que tenha o atributo data-target definido.
 *  @param {string} panel - Id do painel
 *  @param {string} collapse - Status do painel.
 */
BootWrapper.prototype.CollapsePanel = function (panel, collapse) {

    var show = this.IsVarDefined(collapse) && collapse == true;

    var boxTitle = $('#' + panel).find('.panel-heading');
    var dataTarget = boxTitle.attr('data-target');

    if (!this.IsVarDefined(dataTarget))
        return;

    if (show) {
        $(dataTarget).collapse('show');
        boxTitle.removeClass('collapsed');      

    } else {
        $(dataTarget).collapse('hide');
        boxTitle.addClass('collapsed');
    }
};

BootWrapper.prototype.RedirectToURL = function (url) {

    window.document.location.href = url;
};

BootWrapper.prototype.OpenLoading = function () {
    window.bootwrapper.bwHelpers.OpenLoading();
}

BootWrapper.prototype.CloseLoading = function () {
    window.bootwrapper.bwHelpers.CloseLoading();
}

BootWrapper.prototype.ShowErrorMessage = function (message) {
    window.bootwrapper.bwHelpers.ShowErrorMessage(message);
}

/**
 * Iniciar os objetos do framework BootWrapper.
 *  @param {string} defaultErrorPlaceholder - Local onde mensagens de validação será disponibilizadas.
 */
BootWrapper.prototype.Init = function (defaultErrorPlaceholder) {
        
    window.bootwrapper.bwValidation.InitForms(defaultErrorPlaceholder);
    window.bootwrapper.bwTypeahead.Init();
    window.bootwrapper.bwDataTable.Init();
    window.bootwrapper.bwMask.Init();
    window.bootwrapper.base.InitDatePicker();
    window.bootwrapper.base.InitClockPicker();
};

// Declara objeto BootWrapper na classe principal
BootWrapperFW.prototype.base = new BootWrapper();

// cria funções estáticas.
BootWrapper.Log = window.bootwrapper.base.Log;
BootWrapper.ClassOf = window.bootwrapper.base.ClassOf;
BootWrapper.IsVarDefined = window.bootwrapper.base.IsVarDefined;
BootWrapper.AjaxErrorHandler = window.bootwrapper.base.AjaxGenericError;

// Métodos extendidos para o JQuery

(function( $ ){

    // CRIA MÉTODOS para serem usados apenas por $.

    $.bwRedirect = function (url) {
        new BootWrapper().RedirectToURL(url);
    };

    $.bwStartBootWrapper = function () {
        new BootWrapper().Init();
    };

    $.bwCollapsePanel = function (panel, collapse) {
        new BootWrapper().CollapsePanel(panel, collapse);
    };

    $.bwShowModal = function (divmodalId) {
        new BootWrapper().ShowModal(divmodalId);
    };

    $.bwClearDropdownAction = function (container) {
        new BootWrapper().ClearDropdownAction(container);
    };

    $.bwFilterDropdownAction = function (container, actionUrl, jsonData, fnCallback) {
        new BootWrapper().FilterDropdownAction(container, actionUrl, jsonData, fnCallback);
    };

    $.bwLoadAction = function (actionUrl, jsonData, container, fnCallback) {
        new BootWrapper().LoadAction(actionUrl, jsonData, container, fnCallback);
    };

    $.bwExecuteAction = function (actionUrl, jsonData, messageContainer, fnCallback, method) {
        new BootWrapper().ExecuteAction(actionUrl, jsonData, messageContainer, fnCallback, method);
    };

    $.bwSubmitForm = function (formId) {
        new BootWrapper().SubmitForm(formId);
    }

    $.bwSaveForm = function (formId, messageContainer, fnCallback) {
        new BootWrapper().SaveForm(formId, messageContainer, fnCallback);
    }

    $.bwExecuteActionWithForm = function (formId, formAction, messageContainer, fnCallback, fnParam, formMethod) {        
        new BootWrapper().ExecuteActionWithForm(formId, formAction, messageContainer, fnCallback, fnParam, formMethod);
    }

    $.bwReplaceMessages = function (placeholder, messageType, messageList) {
        new BootWrapper().ReplaceMessages(placeholder, messageType, messageList);
    }

    $.bwShowLoading = function () {
        new BootWrapper().OpenLoading();
    }

    $.bwCloseLoading = function () {
        new BootWrapper().CloseLoading();
    }

    $.bwShowMessage = function (mensagem) {        
        new BootWrapper().ShowErrorMessage(mensagem);
    }

    // Funçoes
    // Ex: $('#form').BWSubmitForm();
    $.fn.bwSubmitForm = function () {
        return this.each(function () {
            new BootWrapper().SubmitForm($(this).attr('id'));
        });
    }

    $.fn.bwSaveForm = function (messageContainer, fnCallback) {
        return this.each(function () {
            new BootWrapper().SaveForm($(this).attr('id'), messageContainer, fnCallback);
        });
    }

    $.fn.bwExecuteActionWithForm = function (formAction, messageContainer, fnCallback, fnParam, formMethod) {
        return this.each(function () {
            new BootWrapper().ExecuteActionWithForm($(this).attr('id'), formAction, messageContainer, fnCallback, fnParam, formMethod);
        });
    }

    $.fn.bwCollapsePanel = function (status) {
        return this.each(function () {
            new BootWrapper().CollapsePanel($(this).attr('id'), status);
        });
    }

    $.fn.bwReplaceMessages = function (messageType, messageList) {
        return this.each(function () {
            new BootWrapper().ReplaceMessages($(this).attr('id'), messageType, messageList);
        });
    }

    $.fn.bwClearDropdownAction = function () {
        return this.each(function () {
            new BootWrapper().ClearDropdownAction($(this).attr('id'));
        });
    };

    $.fn.bwFilterDropdownAction = function (actionUrl, jsonData, fnCallback) {
        return this.each(function () {
            new BootWrapper().FilterDropdownAction($(this).attr('id'), actionUrl, jsonData, fnCallback);
        });
    };

})(jQuery);


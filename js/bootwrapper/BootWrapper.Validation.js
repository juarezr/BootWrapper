/// Classe responsável em conter os programas java Scripts que interagem com os objetos Helpers do MVC
/// Qualquer customização para CRUD's especificos deve ser tratado em um JS a parte
/// JavaScript Design Patterns (Prototype Pattern) - ver Learning JavaScript Design Patterns A book by Addy Osmani Volume 1.5.2
/// <reference path="~/scripts/bootwrapper/bootwrapper.Base.js" />
/// <reference path="~/scripts/bootwrapper/bootwrapper.Init.js" />
function BootWrapperValidation() {

    this.MessageType = Object.create(BootWrapperValidation.MessageType);
    this.defaultErrorPlaceholder = 'message-container';

    this.SetDefaultContainer = function (container) {
        if (container !== undefined)
            this.defaultErrorPlaceholder = container;
        else
            this.defaultErrorPlaceholder = 'message-container';
    }
}

BootWrapperValidation.MessageType = {
    Success: 'success',
    Error: 'danger',
    Warning: 'warning',
    Info: 'info'
};

GetMessageTypeIcon = function (messageType) {
    if (messageType == BootWrapperValidation.MessageType.Error)
        return 'fa fa-times-circle-o fa-fw';
    if (messageType == BootWrapperValidation.MessageType.Warning)
        return 'fa fa-warning fa-fw';
    if (messageType == BootWrapperValidation.MessageType.Success)
        return 'fa fa-check-circle-o fa-fw';
    return 'fa fa-info-circle-o fa-fw';
}


BootWrapperValidation.prototype.HighlightControlGroup = function (element, errorClass, validClass) {
    $(element).parent(".form-group").first().addClass("has-error");
};

BootWrapperValidation.prototype.UnhighlightControlGroup = function (element, errorClass, validClass) {
    $(element).parent(".form-group").first().removeClass("has-error");
    $(element).popover('destroy');
};

BootWrapperValidation.prototype.AddErrorDisplay = function (label, element) {
    var text = $(label).html();
    text = '<span class="' + GetMessageTypeIcon(BootWrapperValidation.MessageType.Error) + '" icon-large"></span>' + text;

    //var lbl = '<label class="control-label" for="' + $(element).attr('id') + '">' + text + '</label>';
    $(element).popover({ html: true, trigger: 'focus', title: 'Valor inv&aacute;lido', content: text, placement: 'auto left' });
};

BootWrapperValidation.prototype.AddErrorMessage = function (placeholder, messageText, messageId) {
    return window.bootwrapper.bwValidation.AddMessage(placeholder, BootWrapperValidation.MessageType.Error, messageText, messageId);
}

BootWrapperValidation.prototype.AddWarningMessage = function (placeholder, messageText, messageId) {
    return window.bootwrapper.bwValidation.AddMessage(placeholder, BootWrapperValidation.MessageType.Warning, messageText, messageId);
}

BootWrapperValidation.prototype.AddInfoMessage = function (placeholder, messageText, messageId) {
    return window.bootwrapper.bwValidation.AddMessage(placeholder, BootWrapperValidation.MessageType.Info, messageText, messageId);
}

BootWrapperValidation.prototype.AddSuccessMessage = function (placeholder, messageText, messageId) {
    return window.bootwrapper.bwValidation.AddMessage(placeholder, BootWrapperValidation.MessageType.Success, messageText, messageId);
}

BootWrapperValidation.prototype.AddMessage = function (placeholder, messageType, messageText, messageId) {
    var messageDiv = $('#messages-' + messageType);
    if (messageDiv.length < 1) {
        // Criar placeholder de mensagens        
        var s = '<div id="messages-%1" class="alert alert-%2 alert-dismissible" style="display: block;" role="alert">'
                  + '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>'
                  + '<ul style="list-style: none;"></ul></div>';
        s = BootWrapper.Strings.format(s, messageType, messageType);
        $('#' + placeholder).append(s);
        messageDiv = $('#messages-' + messageType).first();
    } else {
        messageDiv = messageDiv.first();
    }

    var messageIcon = '<span class="fa fa-arrow-right"></span> ';

    var ultag = messageDiv.children('ul').first();
    if (messageId !== undefined) {
        // replace the message
        var li = $('#' + messageId).first();
        if (li.length > 0) {
            li.html(messageIcon + messageText);
        } else {
            ultag.append('<li id="' + messageId + '">' + messageIcon + messageText + '</li>');
        }
    } else {
        // add the message
        ultag.append('<li>' + messageIcon + messageText + '</li>');
    }
    return messageDiv;
};

BootWrapperValidation.prototype.DisplayErrors = function (errorMap) {

    if (BootWrapper.Types.isEmpty(errorMap)) {
        return false;
    }

    var err = 'Alguns campos n&atilde;o foram preenchidos corretamente: <br/>'
                + '<ul style="list-style: none;">';

    var iconError = GetMessageTypeIcon(BootWrapperValidation.MessageType.Error);
    for (var fieldname in errorMap) {
        var errortext = errorMap[fieldname];
        var input = $("input[name='" + fieldname + "']");
        var select = $("select[name='" + fieldname + "']");

        var descr;
        if (input.length > 0) {
            var inputid = input.first().attr('name');
            descr = $("label[for='" + inputid + "']").first().html();
            err = err + '<li><span class="' + iconError + '"></span> ' + descr + ': ' + errortext + '</li>';
        }
        else if (select.length > 0) {
            var selectid = select.first().attr('id');
            descr = $("label[for='" + selectid + "']").first().html();
            err = err + '<li><span class="' + iconError + '"></span> ' + descr + ': ' + errortext + '</li>';
        }
        else {
            err = err + '<li><span class="' + iconError + '"></span> ' + fieldname + ': ' + errortext + '</li>';
        }
    }
    err = err + '</ul></li>';

    this.AddMessage(this.defaultErrorPlaceholder, this.MessageType.Error, err, 'formvalidation');
    window.location.hash = '#' + this.defaultErrorPlaceholder;
        
    return true;
};

BootWrapperValidation.prototype.InitForms = function (msgContainer) {

    
    this.SetDefaultContainer(msgContainer);

    $(document).ready(function () {

        $.validator.setDefaults({
            highlight: window.bootwrapper.bwValidation.HighlightControlGroup,
            unhighlight: window.bootwrapper.bwValidation.UnhighlightControlGroup,
            errorPlacement: window.bootwrapper.bwValidation.AddErrorDisplay,
            showErrors: function (errorMap, errorList) {
                window.bootwrapper.bwValidation.DisplayErrors(errorMap);
                this.defaultShowErrors();
            },
            ignore: "input[type='text']:hidden",
            onfocusout: false,
            onkeyup: false,
            onkeypress: false,
            onclick: false
        });
    });

    $.extend($.validator, {

        messages: {
            required: "Campo obrigat&oacute;rio.",
            remote: "Corrija este campo.",
            email: "Entre um email v&acute;lido.",
            url: "Entre uma URL v&acute;lida.",
            date: "Entre uma data v&acute;lida.",
            dateISO: "Please enter a valid date (ISO).",
            number: "Entre com um número v&acute;lido.",
            digits: "Some d&icute;gitos.",
            creditcard: "Cart&atilde;o de cr&ecute;dito inv&acute;lido.",
            equalTo: "Entre com o mesmo valor.",
            maxlength: $.validator.format("N&atilde;o exceda {0} caracteres."),
            minlength: $.validator.format("Informe no m&iacute;nimo{0} caracteres."),
            rangelength: $.validator.format("Entre um valor entre {0} e {1} caracteres."),
            range: $.validator.format("Informe valor entre {0} e {1}."),
            max: $.validator.format("Informe valor menor igual a {0}."),
            min: $.validator.format("Informe valor maior igual a {0}.")
        }
    });


    $("form .validate").validate();
};


BootWrapperFW.prototype.bwValidation = new BootWrapperValidation();


(function ($) {

    // error message
    $.bwAddErrorMessage = function (placeholder, messageText, messageId) {
        new BootWrapperValidation().AddErrorMessage(placeholder, messageText, messageId);
    };

    $.fn.bwAddErrorMessage = function (messageText, messageId) {
        return this.each(function () {
            new BootWrapperValidation().AddErrorMessage($(this).attr('id'), messageText, messageId);
        });
    }

    // warning
    $.bwAddWarningMessage = function (placeholder, messageText, messageId) {
        new BootWrapperValidation().AddWarningMessage(placeholder, messageText, messageId);
    };

    $.fn.bwAddWarningMessage = function (messageText, messageId) {
        return this.each(function () {
            new BootWrapperValidation().AddWarningMessage($(this).attr('id'), messageText, messageId);
        });
    }

    // info
    $.bwAddInfoMessage = function (placeholder, messageText, messageId) {
        new BootWrapperValidation().AddInfoMessage(placeholder, messageText, messageId);
    };

    $.fn.bwAddInfoMessage = function (messageText, messageId) {
        return this.each(function () {
            new BootWrapperValidation().AddInfoMessage($(this).attr('id'), messageText, messageId);
        });
    }

    // success
    $.bwAddSuccessMessage = function (placeholder, messageText, messageId) {
        new BootWrapperValidation().AddSuccessMessage(placeholder, messageText, messageId);
    };

    $.fn.bwAddSuccessMessage = function (messageText, messageId) {
        return this.each(function () {
            new BootWrapperValidation().AddSuccessMessage($(this).attr('id'), messageText, messageId);
        });
    }
    
    // generic
    $.bwAddMessage = function (placeholder, messageType, messageText, messageId) {
        new BootWrapperValidation().AddMessage(placeholder, messageType, messageText, messageId);
    };

    $.fn.bwAddMessage = function (messageType, messageText, messageId) {
        return this.each(function () {
            new BootWrapperValidation().AddMessage($(this).attr('id'), messageType, messageText, messageId);
        });
    }

})(jQuery)
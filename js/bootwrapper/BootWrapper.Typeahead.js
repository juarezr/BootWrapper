/**
 * Classe responsável pelas funções de Typeahead.
 */
/// <reference path="~/scripts/bootwrapper/bootwrapper.Init.js" />
/// <reference path="~/scripts/bootwrapper/bootwrapper.Base.js" />
function BootWrapperTypeahead() {

}


BootWrapperTypeahead.prototype.UpdateTypeaheadFields = function (selection) {

    var that = this;

    BootWrapper.Log("Select : " + selection);
    var update = that.$element.attr('data-lookup-update');
    if (update === undefined) {
        alert('data-lookup-update não definido para este typeahead');
        return null;
    }

    var item = that.$element.lookupdata[selection];
    if (item === undefined) {
        alert(selection + ' não encontrado na pesquisa para este typeahead');
        return null;
    }

    try {
        var fields = $.parseJSON(update);

        for (var inputname in fields) {
            var input = $('input[name=' + inputname + ']');
            if (input.length != 1) {
                alert(inputname + ' não encontrado na página.');
                return null;
            }
            var fieldname = fields[inputname];
            if (item[fieldname] == undefined) {
                alert(fieldname + ' não encontrado no resultado.');
                return null;
            }
            input.val(item[fieldname]);
        }
    } catch (ex) {
        BootWrapper.Log("Erro convertendo data-lookup-update: não é um formato JSON valido");
        BootWrapper.Log(ex);
    }

    that.$element.lastselection = selection;

    //dont forget to return the item to reflect them into input
    return selection;
};

BootWrapperTypeahead.prototype.CleanTypeaheadFields = function (element) {
    var that = this;
    if ($(that).val() == "") {
        var update = $(element.target).attr('data-lookup-update');
        if (update !== undefined) {
            try {
                var fields = $.parseJSON(update);

                for (var inputname in fields) {
                    $('input[name=' + inputname + ']').val("");
                }
            } catch (ex) {
                BootWrapper.Log("Erro convertendo data-lookup-update: não é um formato JSON valido");
                BootWrapper.Log(ex);
            }
            
            return true;
        }
    }
    return false;
};

BootWrapperTypeahead.prototype.LookupTypeaheadText = function (query, process) {
    
    var that = this;

    // Optimization: reduce ajax calls on heuristics
    if (typeof that.$element.lastquery != 'undefined') {
        if (that.$element.lastquery == query) {
            BootWrapper.Log("Typeahed aborted: same text : " + query);
            return false;
        }
    }

    if (typeof that.$element.lastselection != 'undefined') {
        if (that.$element.lastselection == query) {
            BootWrapper.Log("Typeahed aborted: same selection : " + query);
            return false;
        }
    }

    if (typeof that.$element.ajaxcompleted != 'undefined') {
        if (!that.$element.ajaxcompleted) {
            BootWrapper.Log("Typeahed aborted: waiting ajax");
            return false;
        }
    }

    var d2 = new Date().getTime();
    if (typeof that.$element.lastAjax != 'undefined') {
        var dif = d2 - that.$element.lastAjax;
        if (dif < 1200) {
            BootWrapper.Log("Typeahed aborted: waiting time:" + dif);
            return false;
        }
    }

    // Store info between keystrokes
    var action = that.$element.attr('data-lookup-action');
    var field = that.$element.attr('data-lookup-field');
    that.$element.lookup = [];
    that.$element.lookupdata = [];
    that.$element.lastAjax = new Date().getTime();
    that.$element.ajaxcompleted = false;
    that.$element.lastquery = query;


    var target = that.$element;

    // Do the ajax call    
    $.ajax({
        url: action,
        type: 'POST',
        async: true,
        data: { query: query, items: that.options.items },
        success: function (data) {
            $.each(data, function (i, item) {
                var label = item[field];
                target.lookup.push(label);
                target.lookupdata[label] = item;
            });

            target.ajaxcompleted = true;
            return process(target.lookup);
        }
    }).fail(window.BootWrapper.AjaxErrorHandler);
    return true;
};

BootWrapperTypeahead.prototype.Init = function () {

    $(document).ready(function () {
        $('input[data-provide="typeahead"]').typeahead({
            updater: window.bootwrapper.bwTypeahead.UpdateTypeaheadFields,
            source: window.bootwrapper.bwTypeahead.LookupTypeaheadText,
            minLength: 3,
            items: 16
        });

        $('input[data-provide="typeahead"]').change(window.bootwrapper.bwTypeahead.CleanTypeaheadFields);
       
    });  
};


BootWrapperFW.prototype.bwTypeahead = new BootWrapperTypeahead();

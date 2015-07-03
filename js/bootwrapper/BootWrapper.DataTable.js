/// Classe responsável em controlar o layout do datatable
/// Qualquer customização para CRUD's especificos deve ser tratado em um JS a parte
/// JavaScript Design Patterns (Prototype Pattern) - ver Learning JavaScript Design Patterns A book by Addy Osmani Volume 1.5.2
/// <reference path="~/Scripts/datatables/js/jquery.dataTables.js" />
/// <reference path="~/Scripts/BootWrapper/BootWrapper.DataTable.js" />
function BootWrapperDataTable() {

}


/**
 * Realiza uma pesquisa de dados utilizando um form como filtro. 
 * Os dados encontrados serão devolvidos em formato JSon e Action deverá retornar um JsonResult com as colunas que serão mostradas.
 * Segue exemplo de tratamento e retorno do Json na action:
 * <code>
 * 
 *  var json = new
 *  {
 *               grid.sEcho,
 *               iTotalRecords = nNumeroTotalDeRegistros,
 *               iTotalDisplayRecords = nNumeroRegistrosMostradosPorPágina,
 *               aaData = vm.List
 *                    .Select(r => new
 *                    {
 *                        r.Id,
 *                        r.Nome,
 *                        r.Login,                    
 *                        r.Ativo
 *                    }).ToList()
 *  };
 *  return Json(json);
 * 
 * <code>
 * 
 * Requisitos:
 * 1. O controller deve receber como argumento um objeto do tipo DataTableParameters onde o DataTable será serializado.
 * 2. O form precisa conter o atributo data-action='ajax'.
 * 3. O datatable precisa estar declarado da seguinte forma:
 *      <code> 
 *          <table id="dataTable" data-table="true" class="table table-striped">
 *      </code>
 * 
 * Suporte de formatação para as colunas no header:
 * 1. data-action-link='url', um link é criado com a urk informada. Ex, link para edição do objeto:
 * <th data-field="Id" data-action-link="@url/Edit/{Id}">Código</th>
 * 
 * 2. data-action-function, chamada para função javascript.
 * <th data-field="Id" data-action-function="function_name({Id})">Função Javascript</th>
 * 
 * 3. data-display-date, formatação de data.
 * <th data-field="Dia" data-display-date="true">Inicio</th>
 * 
 * 4. data-display-checkbox, formata célula em checkbox. A propriedade do objeto da célula será convertido para boolean.
 * <th data-field="EstaAtivo" data-display-checkbox="true">Inicio</th>
 * 
 * 5. data-action-button, formata célula com botão. Deverá ser informado a funcão javascript para tratamento do click do botão. 
 * O atributo data-display-format é utilizado para definir o tipo e estilo do botão.
 * 
 * <th data-field="Id" data-action-button="Editar('{Id}')" data-display-format="Editar"></th>
 * <th data-field="Id" data-action-button="Exluir('{Id}')" data-display-format="Excluir"></th>
 * 
 *  @param {string} actionUrl  - URL contendo o controller e action para realizar o request.
 *  @param {json} jsonData     - JSon enviado para a action do controller.
 *  @param {string} messageContainer - Id do container que receberá as mensagens de execução.
 *  @param {string} fnCallback - Função que executará após o processamento. 
 *  @param {string} method - POST ou GET. Se não informado, POST será utilizado.
 */
BootWrapperDataTable.prototype.FindWithForm = function (formId, tableId, fnCallback) {

    function mRenderCheckboxColumn(data, datatype, jsonObject, cellInfo) {

        for (var coli in aoColumns) {
            // Procura a coluna comparando o valor de 'data' e das propriedades do json
            var oCol = aoColumns[coli];
            if ((typeof oCol.mRender != 'undefined') &&
                (oCol.mRender == mRenderCheckboxColumn) &&
                (typeof jsonObject[oCol.mData] != 'undefined') &&
                (data == jsonObject[oCol.mData]) &&
                (coli == cellInfo.col)) {

                if (data == true)
                    return '<input type="checkbox" disabled="disabled" checked="checked">';

                return '<input type="checkbox" disabled="disabled">';

            }
        }
        return 'TH sem data-display-checkbox: ' + data;
    }

    function mRenderActionLinkColumn(data, datatype, jsonObject, cellInfo) {

        for (var coli in aoColumns) {
            // Procura a coluna comparando o valor de 'data' e das propriedades do json
            var oCol = aoColumns[coli];
            if ((typeof oCol.mRender != 'undefined') &&
                (oCol.mRender == mRenderActionLinkColumn) &&
                (typeof jsonObject[oCol.mData] != 'undefined') &&
                (data == jsonObject[oCol.mData]) &&
                (coli == cellInfo.col) &&
                (typeof oCol.mLink != 'undefined')) {                
                
                var link = window.BootWrapper.Strings.formatObject(oCol.mLink, jsonObject);
                var display = oCol.mDisplay ? window.BootWrapper.Strings.formatObject(oCol.mDisplay, jsonObject) : data;

                return '<a href="' + link + '">' + display + '</a>';
            }
        }
        return 'TH sem data-action-link: ' + data;
    }

    function mRenderActionFunctionColumn(data, datatype, jsonObject, cellInfo) {

        for (var coli in aoColumns) {
            // Procura a coluna comparando o valor de 'data' e das propriedades do json
            var oCol = aoColumns[coli];
            if ((typeof oCol.mRender != 'undefined') &&
                (oCol.mRender == mRenderActionFunctionColumn) &&
                (typeof jsonObject[oCol.mData] != 'undefined') &&
                (coli == cellInfo.col) &&
                (data == jsonObject[oCol.mData]) &&
                (typeof oCol.mLink != 'undefined')) {
                var link = window.BootWrapper.Strings.formatObject(oCol.mLink, jsonObject);
                var display = oCol.mDisplay ? window.BootWrapper.Strings.formatObject(oCol.mDisplay, jsonObject) : data;
                return '<a href="#" onclick="' + link + '">' + display + '</a>';
            }
        }
        return 'TH sem data-action-function: ' + data;
    }

    function mRenderActionButtonColumn(data, datatype, jsonObject, cellInfo) {

        for (var coli in aoColumns) {
            // Procura a coluna comparando o valor de 'data' e das propriedades do json
            var oCol = aoColumns[coli];
            if ((typeof oCol.mRender != 'undefined') && 
                (oCol.mRender == mRenderActionButtonColumn) &&
                (typeof jsonObject[oCol.mData] != 'undefined') &&
                (coli == cellInfo.col) &&
                (data == jsonObject[oCol.mData]) && 
                (typeof oCol.mLink != 'undefined')) {
                var link = window.BootWrapper.Strings.formatObject(oCol.mLink, jsonObject);
                var size = (typeof oCol.mSize == 'undefined') ? 'btn-xs' : oCol.mSize;
                var color = (typeof oCol.mColor == 'undefined') ? 'btn-info' : oCol.mColor;
                var ico = '<span class="fa %1 fa-fw"></span>';
                var icon = (typeof oCol.mIcon == 'undefined') ? '' : 
                    window.BootWrapper.Strings.format(ico, oCol.mIcon);
                var tag = '<a class="btn %1 %2" onclick="%3" role="button" test>%4%5</a>';
                tag = window.BootWrapper.Strings.format(tag, size, color, link, icon, oCol.mDisplay);

                return tag;

                //                if (oCol.mDisplay == 'Excluir')
                //                    return '<a class="btn btn-xs btn-danger" onclick="' + link + '" role="button" ><span class="fa fa-trash-o fa-fw"></span>' + oCol.mDisplay + '</a>';
                //return '<a class="btn btn-xs btn-info" onclick="' + link + '" role="button" ><span class="fa fa-pencil fa-fw"></span>' + oCol.mDisplay + '</a>';
            }
        }
        return 'TH sem data-action-button: ' + data;
    }
    
    function mRenderActionDeleteColumn(data, datatype, jsonObject, cellInfo) {

        for (var coli in aoColumns) {
            // Procura a coluna comparando o valor de 'data' e das propriedades do json
            var oCol = aoColumns[coli];
            if ((typeof oCol.mRender != 'undefined') &&
                (oCol.mRender == mRenderActionDeleteColumn) &&
                (typeof jsonObject[oCol.mData] != 'undefined') &&
                (coli == cellInfo.col) &&
                (data == jsonObject[oCol.mData])) {
                var link = window.BootWrapper.Strings.formatObject(oCol.mLink, jsonObject);
                var itemText = window.BootWrapper.Strings.formatObject(oCol.mItemColumnName, jsonObject);
                var size = (typeof oCol.mSize == 'undefined') ? 'btn-xs' : oCol.mSize;
                var color = (typeof oCol.mColor == 'undefined') ? 'btn-danger' : oCol.mColor;
                var ico = '<span class="fa %1 fa-fw"></span>';
                var icon = (typeof oCol.mIcon == 'undefined') ? '' :
                    window.BootWrapper.Strings.format(ico, oCol.mIcon);
                var tag = '<a class="btn %1 %2 lnk-delete" href="#" data-href="%3" role="button" data-toggle="modal" data-target="#confirm-delete" data-item-name="%6">%4%5</a>';
                tag = window.BootWrapper.Strings.format(tag, size, color, link, icon, oCol.mDisplay, itemText);
                return tag;
            }
        }
        return 'TH sem data-action-button: ' + data;
    }

    function mRenderDateColumn(data, datatype, jsonObject) {

        function formatNum(num) {
            if (num < 10)
                return "0" + num;
            else
                return num;
        }
        for (var coli in aoColumns) {
            // Procura a coluna comparando o valor de 'data' e das propriedades do json
            var oCol = aoColumns[coli];
            if ((typeof oCol.mRender != 'undefined') &&
                (oCol.mRender == mRenderDateColumn) &&
                (typeof jsonObject[oCol.mData] != 'undefined') &&
                (data == jsonObject[oCol.mData])) {

                var display = eval(data.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
                var novaData = new Date(display);
                return (formatNum(novaData.getDate()) + "/" + formatNum(novaData.getMonth() + 1) + "/" + formatNum(novaData.getFullYear()) + " " + formatNum(novaData.getHours()) + ":" + formatNum(novaData.getMinutes()) + ":" + formatNum(novaData.getSeconds()));
            }
        }
        return 'TH sem data-display-date: ' + data;
    }

    function mRenderTemplate(data, datatype, jsonObject, cellInfo) {

        for (var coli in aoColumns) {
            // Procura a coluna comparando o valor de 'data' e das propriedades do json
            var oCol = aoColumns[coli];
            if ((typeof oCol.mRender != 'undefined') &&
                (oCol.mRender == mRenderTemplate) &&
                (typeof jsonObject[oCol.mData] != 'undefined') &&
                (coli == cellInfo.col) &&
                (data == jsonObject[oCol.mData]) &&
                (oCol.mTemplateFunction != 'undefined'))
            {
                if (!window.bootwrapper.bwDataTable.MyFunctions[oCol.mTemplateFunction])
                {
                    window.bootwrapper.Log('Função para renderizar coluna não encontrada: ' + oCol.mTemplateFunction);
                    return '';
                }

                return window.bootwrapper.bwDataTable.MyFunctions[oCol.mTemplateFunction](oCol, data, jsonObject, cellInfo);
                //return window[oCol.mTemplateFunction](oCol, data, datatype, jsonObject, cellInfo);
            }
        }
        return 'TH sem data-action-function: ' + data;
    }

    // END Render


    window.BootWrapper.Log(window.BootWrapper.Strings.format("Finding data with params from %1", formId));
    var form = $('#' + formId).first();

    // Verifica se há alguma validação incorreta com o JQuery.Validate
    if (form.attr("data-validate")) {
        if (!form.valid()) {
            return {};
        }
    }

    var dataAction = form.attr("data-action");
    if (dataAction.toLowerCase() == 'submit') {
        form.submit();
    } else {
        var formAction = form.attr("action");
        var formMethod = form.attr("method");
        if (formMethod.toUpperCase() != 'GET') {
            formMethod = 'POST';
        }

        // Pega os campos do TH da tabela com a propriedade data-field
        var aoColumns = [];
        var ths = $('#' + tableId + ' th');
        $.each(ths, function (i, field) {
            var datafield = $(field).attr("data-field");
            var headerTitle = $(field).attr("data-header") || '';
            var col = { "mData": datafield, "sName": headerTitle };

            var displayFormat = $(field).attr("data-display-format");
            if (displayFormat) {
                col.mDisplay = displayFormat;
            }

            var actionButton = $(field).attr("data-action-button");
            var actionIcon = $(field).attr("data-action-icon");
            var actionColor = $(field).attr("data-action-color");
            var actionSize = $(field).attr("data-action-size");

            var actionFunction = $(field).attr("data-action-function");
            var actionlink = $(field).attr("data-action-link");
            var actionTemplate = $(field).attr("data-template");

            var displayDate = $(field).attr("data-display-date");
            var displayCheckbox = $(field).attr("data-display-checkbox");
            var actionDelete = $(field).attr("data-delete");
            
            if (actionTemplate)
            {
                col.mRender = mRenderTemplate;
                col.mTemplateFunction = actionTemplate;
            }
            else if (actionButton) {
                col.mRender = mRenderActionButtonColumn;
                col.mLink = actionButton;
                col.mIcon = actionIcon;
                col.mColor = actionColor;
                col.mSize = actionSize;
            }
            else if (actionDelete) {
                col.mRender = mRenderActionDeleteColumn;
                col.mItemColumnName = $(field).attr("item-column-Name");
                col.mLink = actionDelete;
                col.mIcon = actionIcon;
                col.mColor = actionColor;
                col.mSize = actionSize;
            }
            else if (actionlink) {                
                col.mRender = mRenderActionLinkColumn;
                col.mLink = actionlink;                
            }
            else if (actionFunction) {
                col.mRender = mRenderActionFunctionColumn;
                col.mLink = actionFunction;
            }
            else if (displayDate) {
                col.mRender = mRenderDateColumn;
            }
            else if (displayCheckbox) {
                col.mRender = mRenderCheckboxColumn;
            }

            aoColumns.push(col);
        });

        var serverParams = form.serializeArray();

        window.bootwrapper.bwHelpers.OpenLoading();

        try {
            var grid = $('#' + tableId).dataTable({
                "bDestroy": true,
                "bServerSide": true,
                "bAutoWidth": false,
                "bLengthChange": true,
                "sAjaxSource": formAction,
                "sServerMethod": formMethod,
                "aoColumns": aoColumns,
                "fnServerData": this.fnDataTablesPipeline,
                "fnServerParams": function (aoData) {
                    for (var name in serverParams) {
                        var param = serverParams[name];
                        aoData.push(
                            {
                                "name": param.name,
                                "value": param.value
                            });
                    }
                }
            });

            grid.removeAttr('style');

            // hook to change page
            if (typeof fnCallback != 'undefined') {
                fnCallback(grid);
            }

        } finally {
            window.bootwrapper.bwHelpers.CloseLoading();
        }
    }
    return form;
};


/**
 * Função utilizada pelo findWithForm
 */
BootWrapperDataTable.prototype.fnDataTablesPipeline = function (sSource, aoData, fnCallback) {

    function fnSetKey(oData, sKey, mValue) {
        for (var k = 0; k < oData.length; k++) {
            if (oData[k].name == sKey) {
                oData[k].value = mValue;
            }
        }
    }

    function fnGetKey(oData, sKey) {
        for (var k = 0; k < oData.length; k++) {
            if (oData[k].name == sKey) {
                return oData[k].value;
            }
        }
        return null;
    }

    var that = this;
    if (typeof that.oCache == 'undefined') {
        that.oCache = {
            iCacheLower: -1
        };
    }
    that.Errors = [];

    var iPipe = 5; /* Ajust the pipe size */

    var bNeedServer = false;
    var sEcho = fnGetKey(aoData, "sEcho");
    var iRequestStart = fnGetKey(aoData, "iDisplayStart");
    var iRequestLength = fnGetKey(aoData, "iDisplayLength");
    var iRequestEnd = iRequestStart + iRequestLength;
    that.oCache.iDisplayStart = iRequestStart;

    /* outside pipeline? */
    if (that.oCache.iCacheLower < 0 || iRequestStart < that.oCache.iCacheLower || iRequestEnd > that.oCache.iCacheUpper) {
        bNeedServer = true;
    }

    /* sorting etc changed? */
    if (that.oCache.lastRequest && !bNeedServer) {
        for (var i = 0, iLen = aoData.length; i < iLen; i++) {
            if (aoData[i].name != "iDisplayStart" && aoData[i].name != "iDisplayLength" && aoData[i].name != "sEcho") {
                if (aoData[i].value != that.oCache.lastRequest[i].value) {
                    bNeedServer = true;
                    break;
                }
            }
        }
    }

    /* Store the request for checking next time around */
    that.oCache.lastRequest = aoData.slice();

    if (bNeedServer) {
        if (iRequestStart < that.oCache.iCacheLower) {
            iRequestStart = iRequestStart - (iRequestLength * (iPipe - 1));
            if (iRequestStart < 0) {
                iRequestStart = 0;
            }
        }

        var iDisplayLength = iRequestLength * iPipe;
        that.oCache.iCacheLower = iRequestStart;
        that.oCache.iCacheUpper = iRequestStart + (iDisplayLength);
        that.oCache.iDisplayLength = fnGetKey(aoData, "iDisplayLength");
        fnSetKey(aoData, "iDisplayStart", iRequestStart);
        fnSetKey(aoData, "iDisplayLength", iDisplayLength);
        var sServerMethod = this.fnSettings().sServerMethod;
        var sUrl = sSource + '/' + iRequestStart
                    + '-' + (iRequestStart + iDisplayLength);

        $.ajax({
            dataType: "json",
            type: sServerMethod,
            url: sUrl,
            data: aoData,
            async: false,
            success: function (jsonResult) {
                /* Callback processing */
                that.oCache.lastJson = $.extend(true, {}, jsonResult);

                if (that.oCache.iCacheLower != that.oCache.iDisplayStart) {
                    jsonResult.aaData.splice(0, that.oCache.iDisplayStart - that.oCache.iCacheLower);
                }
                jsonResult.aaData.splice(that.oCache.iDisplayLength, jsonResult.aaData.length);
                if (typeof jsonResult.Errors != 'undefined') {
                    that.Errors = jsonResult.Errors;
                }
                fnCallback(jsonResult);
            }
        });
    }
    else {
        var json = $.extend(true, {}, that.oCache.lastJson);
        json.sEcho = sEcho; /* Update the echo for each response */
        json.aaData.splice(0, iRequestStart - that.oCache.iCacheLower);
        json.aaData.splice(iRequestLength, json.aaData.length);
        fnCallback(json);
        return;
    }
}


BootWrapperDataTable.prototype.BootstrapLengthChange = function (oBSettings) {

    function buildWidget(oInSettings) {
        var stdMenu = '<ul class="pagination">';
        var i, iLen;
        var aLengthMenu = oInSettings.aLengthMenu;
        var bName = oInSettings.sTableId + '_blength';

        var iteml, itema, itemid;
        if (aLengthMenu.length == 2 && typeof aLengthMenu[0] === 'object' &&
            typeof aLengthMenu[1] === 'object') {
            for (i = 0, iLen = aLengthMenu[0].length; i < iLen; i++) {
                iteml = aLengthMenu[0][i];
                if (iteml < oInSettings.aoData.length) {
                    itema = oInSettings._iDisplayLength == iteml ? ' class="active"' : "";
                    itemid = bName + iteml;
                    stdMenu += '<li' + itema + ' id=' + itemid + '"><a href="#">' + aLengthMenu[1][i] + '</a></li>';
                }
            }
        } else {
            for (i = 0, iLen = aLengthMenu.length; i < iLen; i++) {
                iteml = aLengthMenu[i];
                if ((iteml < oInSettings.aoData.length) ||
                ((oInSettings.aoData.length == 0) && oInSettings.oFeatures.bServerSide)) {
                    itema = oInSettings._iDisplayLength == iteml ? ' class="active"' : "";
                    itemid = bName + iteml;
                    stdMenu += '<li' + itema + ' id="' + itemid + '"><a href="#">' + iteml + '</a></li>';
                }
            }
        }
        stdMenu += '</ul>';
        return stdMenu;
    }

    var oSettings = oBSettings.oBSettings;
    if (oSettings.oScroll && oSettings.oScroll.bInfinite) {
        return "";
    }

    /* This can be overruled by not using the _MENU_ var/macro in the language variable */
    //var sName = 'name="' + bName + '"';
    var sStdMenu = buildWidget(oSettings);

    var nLength = document.createElement('div');
    nLength.id = oSettings.sTableId + '_blength';
    nLength.className = nLength.id;
    //nLength.className = oSettings.oClasses.sLength;
    //nLength.innerHTML = '<div class="dataTables_blength pagination">' + oSettings.oLanguage.sLengthMenu.replace('_MENU_', sStdMenu) + '</div>';
    nLength.innerHTML = oSettings.oLanguage.sLengthMenu.replace('_MENU_', sStdMenu);

    $('a', nLength).bind('click', function() {
        var iVal = $(this).text();
        oSettings._iDisplayLength = parseInt(iVal, 10);

        /* Update all other length options for the new display */
        var n = oSettings.aanFeatures.B[0];
        $('li', n).removeClass('active');
        var selected = oSettings.sTableId + '_blength' + iVal;
        $('li#' + selected, n).addClass('active');

        /* Redraw the table */
        oSettings.oApi._fnCalculateEnd(oSettings);

        /* If we have space to show extra rows (backing up from the end point - then do so */
        if (oSettings.fnDisplayEnd() == oSettings.fnRecordsDisplay()) {
            oSettings._iDisplayStart = oSettings.fnDisplayEnd() - oSettings._iDisplayLength;
            if (oSettings._iDisplayStart < 0) {
                oSettings._iDisplayStart = 0;
            }
        }

        if (oSettings._iDisplayLength == -1) {
            oSettings._iDisplayStart = 0;
        }

        oSettings.oApi._fnDraw(oSettings);
    });


    $('div', nLength).attr('aria-controls', oSettings.sTableId);

    return nLength;
};

BootWrapperDataTable.prototype.Init = function () {

    /* Set the defaults for DataTables initialisation */
    $.extend(true, $.fn.dataTable.defaults, {
        "sDom": "t<'row noprint'<'col-md-5'Bf><'col-md-7'p>ir>",
        "sPaginationType": "bootstrap",
        "bFilter": false, "bInfo": false, "bDeferRender": true,
        "aLengthMenu": [10, 20, 35, 50],
        "oLanguage": {
            "sLengthMenu": "_MENU_",
            "sEmptyTable": "Sem dados"
        }
    });


    /* Default class modification */
    $.extend($.fn.dataTableExt.oStdClasses, {
        "sWrapper": "dataTables_wrapper form-inline"
    });


    /* API method to get paging information */
    $.fn.dataTableExt.oApi.fnPagingInfo = function (oSettings) {
        return {
            "iStart": oSettings._iDisplayStart,
            "iEnd": oSettings.fnDisplayEnd(),
            "iLength": oSettings._iDisplayLength,
            "iTotal": oSettings.fnRecordsTotal(),
            "iFilteredTotal": oSettings.fnRecordsDisplay(),
            "iPage": Math.ceil(oSettings._iDisplayStart / oSettings._iDisplayLength),
            "iTotalPages": Math.ceil(oSettings.fnRecordsDisplay() / oSettings._iDisplayLength)
        };
    };


    /* Bootstrap style pagination control */
    $.extend($.fn.dataTableExt.oPagination, {
        "bootstrap": {
            "fnInit": function (oSettings, nPaging, fnDraw) {
                //var oLang = oSettings.oLanguage.oPaginate;
                var fnClickHandler = function (e) {
                    e.preventDefault();
                    if (oSettings.oApi._fnPageChange(oSettings, e.data.action)) {
                        fnDraw(oSettings);
                    }
                };

                $(nPaging).append(
                    '<ul class="pagination" style="float: left">' +
                        '<li class="prev disabled"><a href="#">&laquo; </a></li>' +
                        '<li class="next disabled"><a href="#">&raquo; </a></li>' +
                    '</ul>'
                );
                var els = $('a', nPaging);
                $(els[0]).bind('click.DT', { action: "previous" }, fnClickHandler);
                $(els[1]).bind('click.DT', { action: "next" }, fnClickHandler);
            },

            "fnUpdate": function (oSettings, fnDraw) {
                var iListLength = 5;
                var oPaging = oSettings.oInstance.fnPagingInfo();
                var an = oSettings.aanFeatures.p;
                var i, j, sClass, iStart, iEnd, iHalf = Math.floor(iListLength / 2);

                if (oPaging.iTotalPages < iListLength) {
                    iStart = 1;
                    iEnd = oPaging.iTotalPages;
                }
                else if (oPaging.iPage <= iHalf) {
                    iStart = 1;
                    iEnd = iListLength;
                } else if (oPaging.iPage >= (oPaging.iTotalPages - iHalf)) {
                    iStart = oPaging.iTotalPages - iListLength + 1;
                    iEnd = oPaging.iTotalPages;
                } else {
                    iStart = oPaging.iPage - iHalf + 1;
                    iEnd = iStart + iListLength - 1;
                }

                for (i = 0; i < an.length; i++) {
                    // Remove the middle elements
                    $('li:gt(0)', an[i]).filter(':not(:last)').remove();

                    // Add the new list items and their event handlers
                    for (j = iStart ; j <= iEnd ; j++) {
                        sClass = (j == oPaging.iPage + 1) ? 'class="active"' : '';
                        $('<li ' + sClass + '><a href="#">' + j + '</a></li>')
                            .insertBefore($('li:last', an[i])[0])
                            .bind('click', function (e) {
                                e.preventDefault();
                                oSettings._iDisplayStart = (parseInt($('a', this).text(), 10) - 1) * oPaging.iLength;
                                fnDraw(oSettings);
                            });
                    }

                    // Add / remove disabled classes from the static elements
                    if (oPaging.iPage === 0) {
                        $('li:first', an[i]).addClass('disabled');
                    } else {
                        $('li:first', an[i]).removeClass('disabled');
                    }

                    if (oPaging.iPage === oPaging.iTotalPages - 1 || oPaging.iTotalPages === 0) {
                        $('li:last', an[i]).addClass('disabled');
                    } else {
                        $('li:last', an[i]).removeClass('disabled');
                    }
                }
            }
        }
    });


    /*
     * TableTools Bootstrap compatibility
     * Required TableTools 2.1+
     */
    if ($.fn.DataTable.TableTools) {
        // Set the classes that TableTools uses to something suitable for Bootstrap
        $.extend(true, $.fn.DataTable.TableTools.classes, {
            "container": "DTTT btn-group",
            "buttons": {
                "normal": "btn",
                "disabled": "disabled"
            },
            "collection": {
                "container": "DTTT_dropdown dropdown-menu",
                "buttons": {
                    "normal": "",
                    "disabled": "disabled"
                }
            },
            "print": {
                "info": "DTTT_print_info modal"
            },
            "select": {
                "row": "active"
            }
        });

        // Have the collection use a bootstrap compatible dropdown
        $.extend(true, $.fn.DataTable.TableTools.DEFAULTS.oTags, {
            "collection": {
                "container": "ul",
                "button": "li",
                "liner": "a"
            }
        });
    }

    $.fn.dataTableExt.aoFeatures.push({
        "fnInit": function (oSettings) {
            return new window.bootwrapper.bwDataTable.BootstrapLengthChange({ "oBSettings": oSettings });
        },
        "cFeature": "B",
        "sFeature": "bootwrapper.bootwrapperDataTable.BootstrapLengthChange"
    });
   
};


BootWrapperFW.prototype.bwDataTable = new BootWrapperDataTable();



// Métodos extendidos para o JQuery

(function ($) {

    $.bwFindWithForm = function (formId, tableId, fnCallback) {
        new BootWrapperDataTable().FindWithForm(formId, tableId, fnCallback);
    };

    $.fn.bwFindWithForm = function (tableId, fnCallback) {
        return this.each(function () {
            new BootWrapperDataTable().FindWithForm($(this).attr('id'), tableId, fnCallback);
        });
    }

})(jQuery)
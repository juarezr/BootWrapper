# BootWrapper

Este projeto tem como objetivo facilitar o udo do Bootstrap.<br/>
Ele possui classes que estendem metódos para a classe HtmlHelper. Todos os métodos iniciam pelas letras BW.<br/>

Todos os componentes podem ser utilizados nas views através da nomenclatura.
<code>@Html.BW*Nome_do_Metodo*</code>

## Install

1. Faça o download do arquivo BootWrapper.BW.dll que se encontra na pasta releases.
2. Adicione a dll no seu projeto.
3. Instale através do Nuget os componentes necessários para utilizaçao do BootWrapper. Se preferir, substitua o arquivo package.config do seu projeto pelo arquivo que se encontra na pasta 'nuget'.
  * Install-Package bootstrap -version 3.34
  * Install-Package Bootstrap.Datepicker -version 1.4.0
  * Install-Package BootstrapBootstrap-3-Typeahead -version 3.1.1
  * Install-Package FontAwesome -version 3.1.1
  * Install-Package FontAwesome.MVC -version 1.0.0
  * Install-Package jQuery -version 2.1.4
  * Install-Package jquery.datatables -version 1.10.7
  * Install-Package jQuery.Validation -version 1.13.1
  * Install-Package Messenger -version 1.4.1
4. Adicione no seu arquivo de lauyout os arquivos de javascript que se encontram na pasta 'js'. Ou crie um bundle:
```C#
    var scriptBundle = new ScriptBundle("~/bundles/bootwrapper");
    scriptBundle.Include("~/Scripts/bootwrapper/BootWrapper.Init.js");
    scriptBundle.Include("~/Scripts/bootwrapper/BootWrapper.Helpers.js");
    scriptBundle.Include("~/Scripts/bootwrapper/BootWrapper.Base.js");
    scriptBundle.Include("~/Scripts/bootwrapper/BootWrapper.Validation.js");
    scriptBundle.Include("~/Scripts/bootwrapper/BootWrapper.Typeahead.js");
    scriptBundle.Include("~/Scripts/bootwrapper/BootWrapper.DataTable.js");            
    scriptBundle.Include("~/Scripts/bootwrapper/BootWrapper.Mask.js");
    bundles.Add(scriptBundle);
```
5. Inicie o BootWrapper
```javascript
      jQuery(document).ready(function () {
          $.bwStartBootWrapper();
      });
```
  

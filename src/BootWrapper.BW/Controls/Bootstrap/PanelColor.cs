
namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Define a cor do painel utilizado na classe <see cref="MvcPanel"/>
    /// </summary>
    public enum PanelColor
    {
        /// <summary>
        /// Cor default do painel de acordo com o Tema utilizado.
        /// </summary>
        [BootstrapValue("panel-default")]
        Default,

        /// <summary>
        /// Cor azul.
        /// </summary>
        [BootstrapValue("panel-primary")]
        Blue,

        /// <summary>
        /// Cor verde.
        /// </summary>
        [BootstrapValue("panel-success")]
        Green,

        /// <summary>
        /// Cor azul claro.
        /// </summary>
        [BootstrapValue("panel-info")]
        LighBlue,
        
        /// <summary>
        /// Cor laranja.
        /// </summary>
        [BootstrapValue("panel-warning")]
        Orange,

        /// <summary>
        /// Cor vermelha.
        /// </summary>
        [BootstrapValue("panel-danger")]
        Red,

        /// <summary>
        /// Cor preta.
        /// </summary>
        [BootstrapValue("panel-inverse")]
        Black,

        /// <summary>
        /// Cor branca.
        /// </summary>
        [BootstrapValue("panel-white")]
        White,
   
        /// <summary>
        /// Sem cor.
        /// </summary>
        [BootstrapValue("")]
        NoColor
    }
}

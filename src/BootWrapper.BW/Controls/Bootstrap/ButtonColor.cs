namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Define as cores de botão encontradas em classes do bootstrap.
    /// </summary>
    public enum ButtonColor
    {
        /// <summary>
        /// Botão personalizado.
        /// </summary>
        [BootstrapValue("btn-custom")]
        Custom,

        /// <summary>
        /// Botão default.
        /// </summary>
        [BootstrapValue("btn-default")]
        NoColor,

        /// <summary>
        /// Botão azul.
        /// </summary>
        [BootstrapValue("btn-primary")]
        Blue,

        /// <summary>
        /// Botão verdade para sucesso.
        /// </summary>
        [BootstrapValue("btn-success")]
        Green,

        /// <summary>
        /// Botão azul-claro para info.
        /// </summary>
        [BootstrapValue("btn-info")]
        LighBlue,

        /// <summary>
        /// Botão laranjado de atenção.
        /// </summary>
        [BootstrapValue("btn-warning")]
        Orange,

        /// <summary>
        /// Botão vermelho de alerta.
        /// </summary>
        [BootstrapValue("btn-danger")]
        Red,

        /// <summary>
        /// Botão sem cor.
        /// </summary>
        [BootstrapValue("btn-link")]
        Transparent,
    }
}

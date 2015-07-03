
namespace BootWrapper.BW.Controls
{
    /// <summary>
    /// Classe para definir o tipo do botão e seus atributos como o ícone utilizado.
    /// </summary>
    public enum ButtonAction
    {
        /// <summary>
        /// Icone para o botão "adicionar"
        /// </summary>
        [BootstrapValue("fa fa-plus")]
        Add,

        /// <summary>
        /// Icone para o botão "deletar"
        /// </summary>
        [BootstrapValue("fa fa-minus")]
        Del,

        /// <summary>
        /// Icone para o botão "editar"
        /// </summary>
        [BootstrapValue("fa fa-pencil")]
        Edit,

        /// <summary>
        /// Icone para o botão "remover"
        /// </summary>
        [BootstrapValue("fa fa-trash-o")]
        Remove,

        /// <summary>
        /// Icone para o botão "pesquisar"
        /// </summary>
        [BootstrapValue("fa fa-search")]
        Search,

        /// <summary>
        /// Icone para o botão "atualizar"
        /// </summary>
        [BootstrapValue("fa fa-refresh")]
        Reload,

        /// <summary>
        /// Icone para o botão "salvar"
        /// </summary>
        [BootstrapValue("fa fa-save")]
        Save,

        /// <summary>
        /// Icone para o botão "login"
        /// </summary>
        [BootstrapValue("")]
        Login,

        /// <summary>
        /// Icone para o botão "voltar"
        /// </summary>
        [BootstrapValue("fa fa-arrow-left")]
        Back,
    }
}

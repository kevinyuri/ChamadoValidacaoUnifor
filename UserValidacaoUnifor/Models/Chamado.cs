namespace UserValidacaoUnifor.Models
{
    public class Chamado
    {
        public int Id { get; set; }
        public string? NomeSolicitante { get; set; }
        public string? Setor { get; set; }
        public string? Mensagem { get; set; }
        public string? Status { get; set; }
    }
}

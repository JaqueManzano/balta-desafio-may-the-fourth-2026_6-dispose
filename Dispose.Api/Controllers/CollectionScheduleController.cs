using Microsoft.AspNetCore.Mvc;

namespace Dispose.Api.Controllers
{
    public class CollectionScheduleController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Criar post para cadastrar os itens que o usuário tem para descartar
            return View();
        }
    }
}

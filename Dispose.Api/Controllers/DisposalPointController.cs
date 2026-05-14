using Microsoft.AspNetCore.Mvc;

namespace Dispose.Api.Controllers
{
    public class DisposalPointController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Criar endpoint get para informar os dias pra cada tipo de coleta na rua
            return View();
        }
    }
}

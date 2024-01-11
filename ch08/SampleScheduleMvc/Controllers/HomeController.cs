using Azure.AI.OpenAI;
using Azure;
using Microsoft.AspNetCore.Mvc;
using SampleScheduleMvc.Models;
using System.Diagnostics;

namespace SampleScheduleMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            var vm = new PromptViewModel();
            // 初回の表示
            await vm.SendInit();
            return View(vm);
        }

        public async Task<IActionResult> Edit([Bind("Input,Output")] PromptViewModel vm )
        {
            // 2回目以降の表示
            await vm.Send();
            return View("Index", vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }



}
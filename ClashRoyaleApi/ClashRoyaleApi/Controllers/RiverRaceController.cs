﻿using ClashRoyaleApi.Logic.RiverRace;
using Microsoft.AspNetCore.Mvc;

namespace ClashRoyaleApi.Controllers
{
    public class RiverRaceController : Controller
    {
        private readonly IRiverRaceLogic _riverRaceLogic;

        public RiverRaceController(IRiverRaceLogic riverRaceLogic)
        {
            _riverRaceLogic = riverRaceLogic;
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}

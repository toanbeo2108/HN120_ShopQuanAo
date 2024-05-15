﻿using HN120_ShopQuanAo.API.Service.IServices;
using HN120_ShopQuanAo.API.Service.Services;
using HN120_ShopQuanAo.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HN120_ShopQuanAo.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonService _sv;
        
        public HoaDonController(IHoaDonService sv)
        {
                _sv =sv;
        }
        [HttpGet]
        public IActionResult GetAllHoDon() {
            var hoadon = _sv.GetAllHoaDon();
            return Ok(hoadon);
        }
        [HttpGet("ma:string")]
        public IActionResult GetAllHoDonMa(string ma) {
            try
            {
                var hoadon = _sv.GetHoaDonByMa(ma);
                return Ok(hoadon);
               
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
           
        }
        [HttpPost]
        public IActionResult CreateHoaDon(HoaDon hoaDon)
        {
            try
            {
                _sv.CreateHoaDon(hoaDon);
                return StatusCode(StatusCodes.Status201Created);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
           
        }
        [HttpPut]
        public IActionResult UpdateHoaDon(HoaDon hoaDon)
        {
            try
            {
                _sv.UpdateHoaDon(hoaDon);
                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

           
        }
        [HttpDelete("ma:string")]
        public IActionResult DeleteHoaDon(string ma)
        {
            try
            {
              
                _sv.DeleteHoaDon(ma);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

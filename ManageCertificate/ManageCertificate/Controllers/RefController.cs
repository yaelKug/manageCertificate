﻿
using Microsoft.AspNetCore.Mvc;

using Entites;

using BL.Interfaces;
using BL;
using DTO;
using DAL.Interfaces;

namespace manageCertificate;


[Route("api/[controller]")]
[ApiController]

public class RefController : Controller
{
    ILogger<RefController> logger;
    IRefBL RefBL;
    public RefController(IRefBL RefBL, ILogger<RefController> logger)
    {
        this.RefBL = RefBL;
        this.logger = logger;
    }
    [HttpPost("AddOfficeInventory")]
    public async Task<ActionResult<RefOfficeInventory>> AddOfficeInventory([FromBody] AddRefOfficeInventoryDTO dto)
    {
        if (dto == null)
            return BadRequest(new { message = "הנתונים שנשלחו ריקים" });

        if (!ModelState.IsValid)
            return BadRequest(new { message = "הנתונים שנשלחו אינם תקינים", errors = ModelState });

        try
        {
            var result = await RefBL.AddOfficeInventoryAsync(dto);
            return CreatedAtAction(nameof(AddOfficeInventory), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            // טיפול בשגיאה מותאמת אישית
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            // טיפול בשגיאה כללית
            return StatusCode(500, new { message = "שגיאה בשרת", details = ex.Message });
        }
    }

    [HttpPut("/api/updateInventoryAmount")]
    public async Task<IActionResult> UpdateInventoryAmount([FromBody] UpdateInventoryDTO inventoryDto)
    {
        if (inventoryDto == null)
            return BadRequest();

        var result = await RefBL.UpdateInventoryAmountAsync(inventoryDto.InventoryId, inventoryDto.Inventory);
        if (result == null)
            return NotFound();

        return Ok(result); // מחזיר את האובייקט המעודכן
    }
    [HttpPut("/api/updateMinimum")]
    public async Task<IActionResult> UpdateMinimum([FromBody] UpdateCTMinimumDTO cTMinimumDTO)
    {
        if (cTMinimumDTO == null)
            return BadRequest();

        var result = await RefBL.UpdateMinimum(cTMinimumDTO.CertificateId, cTMinimumDTO.Minimum);
        if (result == null)
            return NotFound();

        return Ok(result); // מחזיר את האובייקט המעודכן
    }
    [HttpGet("/GetAllOfficeInventory")]
    public async Task<ActionResult<IEnumerable<RefOfficeInventory>>> GetAllOfficeInventory()
    {
        IEnumerable<RefOfficeInventory> OfficeInventory = await RefBL.GetAllOfficeInventory();
        return Ok(OfficeInventory);

    }
    [HttpGet("/api/RefStatus")]
    public async Task<ActionResult<IEnumerable<RefStatus>>> GetAllStatus()
    {
        IEnumerable<RefStatus> refStatus = await RefBL.GetAllStatus();
        return Ok(refStatus);
    }
    [HttpGet("/GetAllInventory")]
    public async Task<ActionResult<IEnumerable<RefInventoryDTO>>> GetAllInventory()
    {
        IEnumerable<RefInventoryDTO> inventories = await RefBL.GetAllInventory();
        return Ok(inventories);
    }
    [HttpGet("/GetInventoryById")]
    public async Task<ActionResult<RefInventoryDTO>> GetInventoryById(int concilId,  int certificateId)
    {
        RefInventoryDTO inventory = await RefBL.GetInventoryById(concilId, certificateId);
        return Ok(inventory);
        
    }
    [HttpGet("/GetAllCertificate")]
    public async Task<ActionResult<IEnumerable<CertificateDTO>>> GetAllCertificate()
    {
        IEnumerable<CertificateDTO> certificate = await RefBL.GetAllCertificate();
        return Ok(certificate);
    }
    [HttpGet("/GetAllCertificateType")]
    public async Task<ActionResult<IEnumerable<RefCertificateType>>> GetAllCertificateType()
    {
        IEnumerable<RefCertificateType> certificateType = await RefBL.GetAllCertificateType();
        return Ok(certificateType);
    }
}
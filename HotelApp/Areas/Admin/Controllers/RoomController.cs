using HotelApp.Data;
using HotelApp.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;

namespace HotelApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("Admin")]
        [Route("Manage/Room/Index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("Room/ListRoom")]
        public async Task<IActionResult> ListRoom()
        {
            var rooms = await _context.Rooms
            .Include(r => r.RoomType) // Lấy thông tin RoomType
            .Include(r => r.Area) // Lấy thông tin Area
            .Select(r => new RoomVM
            {
                Id = r.Id,
                RoomTypeName = r.RoomType.Name,
                AreaName = r.Area.Name,
                Price = r.Price,
                Discount = r.Discount,
                Status = r.Status,
                DaDatPhong = _context.Bookings.Any(b => b.RoomID == r.Id) // Kiểm tra phòng đã được đặt chưa
            })
            .ToListAsync();

            return Json(new { Data = rooms });
        }
[HttpGet]
[Route("Room/Create")]
public async Task<IActionResult> Create()
{
        var viewModel = new RoomEditVM
    {
        RoomTypes = await _context.RoomTypes.Select(rt => new RoomType { Id = rt.Id, Name = rt.Name }).ToListAsync(),
        Areas = await _context.Areas.Select(a => new Area { Id = a.Id, Name = a.Name }).ToListAsync(),
        SelectedAmenities = new List<int>(),
        Amenities = await _context.Amenities.ToListAsync(),
    };

    ViewData["Statuses"] = GetStatuses();
    return View(viewModel);
}

[HttpPost]
[Route("Room/Create")]
public async Task<IActionResult> Create(RoomEditVM viewModel, IFormFileCollection newImages)
{

    var room = new Room
    {
        TypeId = viewModel.TypeId,
        AreaId = viewModel.AreaId,
        Price = viewModel.Price,
        Discount = viewModel.Discount,
        Status = viewModel.Status,
        Amenities = await _context.Amenities.Where(a => viewModel.SelectedAmenities.Contains(a.Id)).ToListAsync(),
        Images = new List<Image>()
    };

    if (newImages?.Count > 0)
    {
        foreach (var file in newImages)
        {
            var filePath = Path.Combine("wwwroot/images", file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            room.Images.Add(new Image { Path = "/images/" + file.FileName, Caption = file.FileName });
        }
    }

    _context.Rooms.Add(room);
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}


        [HttpGet]
        [Route("Room/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.Area)
                .Include(r => r.Amenities)
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null) return NotFound();

            var viewModel = new RoomDetailsVM
            {
                Id = room.Id,
                RoomTypeName = room.RoomType.Name,
                AreaName = room.Area.Name,
                Price = room.Price,
                Discount = room.Discount,
                Status = room.Status,
                Amenities = room.Amenities.Select(a => a.Name).ToList(),
                Images = room.Images.ToList()
            };
            ViewData["Statuses"] = GetStatuses();

            return View(viewModel);
        }


        [HttpGet]
        [Route("Room/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.Area)
                .Include(r => r.Amenities)
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null) return NotFound();

            var viewModel = new RoomEditVM
            {
                Id = room.Id,
                TypeId = room.TypeId,
                AreaId = room.AreaId,
                Price = room.Price,
                Discount = room.Discount,
                Status = room.Status,
                RoomTypes = await _context.RoomTypes.Select(rt => new RoomType
                {
                    Id = rt.Id,
                    Name = rt.Name
                }).ToListAsync(),
                Areas = await _context.Areas.Select(a => new Area
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToListAsync(),
                Amenities = await _context.Amenities.ToListAsync(),
                SelectedAmenities = room.Amenities.Select(a => a.Id).ToList(),
                Images = room.Images.ToList()
            };
            ViewData["Statuses"] = GetStatuses();
            return View(viewModel);
        }



        [HttpPost]
        [Route("Room/Edit/{id}")]
        public async Task<IActionResult> Edit(RoomEditVM viewModel, IFormFileCollection newImages)
        {
            var room = await _context.Rooms
                .Include(r => r.Amenities)
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.Id == viewModel.Id);

            if (room == null) return NotFound();

            room.TypeId = viewModel.TypeId;
            room.AreaId = viewModel.AreaId;
            room.Price = viewModel.Price;
            room.Discount = viewModel.Discount;
            room.Status = viewModel.Status;

            // Cập nhật tiện ích
            room.Amenities.Clear();
            var selectedAmenities = await _context.Amenities.Where(a => viewModel.SelectedAmenities.Contains(a.Id)).ToListAsync();
            room.Amenities = selectedAmenities;

            // Thêm ảnh mới
            if (newImages?.Count > 0)
            {
                foreach (var file in newImages)
                {
                    var filePath = Path.Combine("wwwroot/images", file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    room.Images.Add(new Image { Path = "/images/" + file.FileName, Caption = file.FileName });
                }
            }
            ViewData["Statuses"] = GetStatuses();
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    

// Xóa phòng
        [HttpPost]
        [Route("Room/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return Json(new {success = true, message = "Không tìm thấy phòng!" });

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Xóa thành công!" });
        }

        // Hàm lấy danh sách trạng thái
        private List<SelectListItem> GetStatuses()
        {
            return new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "Hoạt động" },
            new SelectListItem { Value = "1", Text = "không hoạt động" },
            new SelectListItem { Value = "-1", Text = "bảo trì" }
        };
        }
        [HttpPost]
        [Route("Room/DeleteImage")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return Json(new { success = false, message = "Ảnh không tồn tại." });
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        [Route("Room/EditImageCaption")]
        public async Task<IActionResult> EditImageCaption(int id, string caption)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return Json(new { success = false, message = "Ảnh không tồn tại." });
            }

            image.Caption = caption;
            _context.Images.Update(image);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }


    }
}

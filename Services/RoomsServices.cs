using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjetoMultidiciplinar.Data;
using ProjetoMultidiciplinar.DTOs;
using ProjetoMultidiciplinar.Models;

namespace ProjetoMultidiciplinar.Services
{
    public class RoomsService
    {
        private readonly AppDbContext _context;
        public RoomsService(AppDbContext context)
        {
            _context = context;
        }

    }
}
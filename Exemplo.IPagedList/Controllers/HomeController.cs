using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Exemplo.IPagedList.Models;
using X.PagedList;

namespace Exemplo.IPagedList.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Home
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var items = new List<SelectListItem>()
            {
                new SelectListItem{ Text = "10", Value = "10" },
                new SelectListItem{ Text = "20", Value = "20" },
                new SelectListItem{ Text = "50", Value = "50" },
                new SelectListItem{ Text = "100", Value = "100" },
                new SelectListItem{ Text = "1000", Value="1000"}
            };
            
            
            ViewBag.dropdown = items;

            ViewBag.CurrentSort = sortOrder;

            ViewBag.NomeSortParam = sortOrder == "nome_asc" ? "nome_desc" : "nome_asc";
            ViewBag.ProfissaoSortParam = sortOrder == "profissao_asc" ? "profissao_desc" : "profissao_asc";
            ViewBag.EnderecoSortParam = sortOrder == "endereco_asc" ? "endereco_desc" : "endereco_asc";
            ViewBag.EmailSortParam = sortOrder == "email_asc" ? "email_desc" : "email_asc";

            int numeroRegistros = 10;

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            if (Request.Form["items"] != null)
                ViewBag.dropdownselecionado = Convert.ToInt32(Request.Form["items"]); 
            else
                ViewBag.dropdownselecionado = numeroRegistros;
            
            ViewBag.CurrentFilter = searchString;

            var consulta = db.Pessoas.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                consulta = consulta.Where(s => s.Nome.Contains(searchString)|| s.Nome.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "nome_asc":
                    consulta = consulta.OrderBy(s => s.Nome);
                    break;
                case "nome_desc":
                    consulta = consulta.OrderByDescending(s => s.Nome);
                    break;

                case "profissao_asc":
                    consulta = consulta.OrderBy(s => s.Profissao);
                    break;
                case "profissao_desc":
                    consulta = consulta.OrderByDescending(s => s.Profissao);
                    break;

                case "endereco_asc":
                    consulta = consulta.OrderBy(s => s.Endereco);
                    break;
                case "endereco_desc":
                    consulta = consulta.OrderByDescending(s => s.Endereco);
                    break;

                case "email_asc":
                    consulta = consulta.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    consulta = consulta.OrderByDescending(s => s.Email);
                    break;

                default:  
                    consulta = consulta.OrderBy(s => s.PessoaId);
                    break;
            }

            int pageSize = numeroRegistros;
            int pageNumber = (page ?? 1);

            ViewBag.PagePessoas = await consulta.ToPagedListAsync(pageNumber, pageSize);
            
            return View();
        }

        // GET: Home/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pessoa pessoa = await db.Pessoas.FindAsync(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PessoaId,Nome,Profissao,Endereco,Email")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                db.Pessoas.Add(pessoa);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pessoa);
        }

        // GET: Home/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pessoa pessoa = await db.Pessoas.FindAsync(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PessoaId,Nome,Profissao,Endereco,Email")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pessoa).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pessoa);
        }

        // GET: Home/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pessoa pessoa = await db.Pessoas.FindAsync(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pessoa pessoa = await db.Pessoas.FindAsync(id);
            db.Pessoas.Remove(pessoa);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

       
        public ActionResult About()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

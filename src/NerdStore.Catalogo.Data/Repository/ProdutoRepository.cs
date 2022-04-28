using Microsoft.EntityFrameworkCore;
using NerdStore.Catalogo.Domain;
using NerdStore.Core.Data;

namespace NerdStore.Catalogo.Data.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CatalogoContext _context;


        public ProdutoRepository(CatalogoContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Produto>> ObterTodos()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }

        public async Task<Produto> ObterPorId(Guid id)
        {
            return await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produto>> ObterPorCategoria(int codigo)
        {
            return await _context.Produtos.AsNoTracking().Include(p => p.Categoria).Where(c => c.Categoria.Codigo == codigo).ToListAsync();
        }

        public async Task<IEnumerable<Categoria>> ObterCategorias()
        {
            var categorias = await _context.Categorias.AsNoTracking().ToListAsync();
            return categorias;
        }

        public void Adicionar(Produto produto)
        {
            _context.Produtos.Add(produto);
        }

        public void Atualizar(Produto produto)
        {
            _context.Produtos.Update(produto);
        }

        public void Adicionar(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
        }

        public void Atualizar(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
        }

        #region Disposable Members

        private bool disposedValue;

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _context?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

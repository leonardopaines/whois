using Base.Data.Model.Entities;
using Base.DataModel.Repositories;
using System;
using System.Diagnostics;

namespace Base.DataModel.UnitOfWork
{
    /// <summary>
    /// Unit of Work class responsible for DB transactions
    /// </summary>
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        #region Private member variables...

        private MySQLContext _context = null;
        private GenericRepository<pesquisa> _pesquisaRepository;
        private GenericRepository<site> _siteRepository;
        #endregion

        public UnitOfWork()
        {
            _context = new MySQLContext();
        }

        #region Public Repository Creation properties...

        public GenericRepository<pesquisa> PesquisaRepository
        {
            get
            {
                if (this._pesquisaRepository == null)
                    this._pesquisaRepository = new GenericRepository<pesquisa>(_context);
                return _pesquisaRepository;
            }
        }

        public GenericRepository<site> SiteRepository
        {
            get
            {
                if (this._siteRepository == null)
                    this._siteRepository = new GenericRepository<site>(_context);
                return _siteRepository;
            }
        }

        #endregion

        #region Public member methods...

        /// <summary>
        /// Save method.
        /// </summary>
        public int Save()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception objException)
            {
                throw new Exception("Erro em UnitOfWork.Save():" + objException.Message, objException);
            }

        }

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool disposed = false;
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
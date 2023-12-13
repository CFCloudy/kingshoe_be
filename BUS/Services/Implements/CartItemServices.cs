using BUS.Services.Interfaces;
using DAL.Models;
using DAL.Repositories.Implements;
using DTO.Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Services.Implements
{
    public class CartItemServices : BaseService<CartItem>
    {
        private GenericRepository<CartItem> _da = new GenericRepository<CartItem>();
        private readonly RequestRepository _requestDA = new RequestRepository();
        public override ReturnObject Create(CartItem entity)
        {
            try
            {
                _da.AddDataCommand(entity);
                return new ReturnObject();
            }
            catch (Exception e)
            {
                return new ReturnObject(e.Message);
            }
        }

        public override ReturnObject Delete(int id)
        {
            try
            {
                _da.DeleteDataCommand(GetById(id));
                return new ReturnObject();
            }
            catch (Exception e)
            {
                return new ReturnObject(e.Message);
            }
        }

        public override IQueryable<CartItem> GetAll()
        {
            return _da.GetAllDataQuery();
        }

        public override CartItem GetById(int id)
        {
            return _requestDA.GetOneCartItemByID(id);
        }

        public override Tuple<List<CartItem>, int> GetByRequest(string searchKey = "", int currentPage = 1, int rowPerPage = 20)
        {
            currentPage--;
            var skip = currentPage * rowPerPage;
            return _requestDA.GetListCartItemsByRequest(searchKey, skip, rowPerPage);
        }

        public override ReturnObject Update(CartItem entity)
        {
            try
            {
                _da.UpdateDataCommand(entity);
                return new ReturnObject();
            }
            catch (Exception e)
            {
                return new ReturnObject(e.Message);
            }
        }
    }
}

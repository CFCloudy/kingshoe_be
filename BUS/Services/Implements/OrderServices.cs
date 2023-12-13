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
    public class OrderServices : BaseService<Order>
    {
        private GenericRepository<Order> _da = new GenericRepository<Order>();
        private readonly RequestRepository _requestDA = new RequestRepository();
        public override ReturnObject Create(Order entity)
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

        public override IQueryable<Order> GetAll()
        {
            return _da.GetAllDataQuery();
        }

        public override Order GetById(int id)
        {
            try
            {
                return _requestDA.GetOneOrderByID(id);
            }
            catch (Exception)
            {
                return new Order();
            }
            
        }

        public override Tuple<List<Order>, int> GetByRequest(string searchKey = "", int currentPage = 1, int rowPerPage = 20)
        {
            currentPage--;
            var skip = currentPage * rowPerPage;
            return _requestDA.GetListOrderByRequest(searchKey, skip, rowPerPage);
        }

        public override ReturnObject Update(Order entity)
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

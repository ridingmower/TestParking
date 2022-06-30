using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Kztek.Service.API
{
    public interface IAPI_CustomerService
    {
        IEnumerable<tblCustomer> GetAll();
        IEnumerable<tblCustomer> GetAllCustomer(string key);
        IEnumerable<tblCustomer_API3rd> GetTop10(string key);
        // IEnumerable<tblCustomerGroup> GetAll_CustomerGroup();
        tblCustomer_API3rd GetByNameOrAdd(string name, string add);
        tblCustomer_API3rd GetByMobileOrIdNumber(string mobile, string IdNumber);
        IEnumerable<tblCustomer_API3rd> GetByMobileOrName(string key);
        MessageReport Create(tblCustomer obj);
        tblCustomer GetById(string id);
        MessageReport Update(tblCustomer obj);
        MessageReport DeleteById(string id);
    }

    public class API_CustomerService : IAPI_CustomerService
    {
        private ItblCustomerRepository _tblCustomerRepository;

        private IUnitOfWork _UnitOfWork;
        public API_CustomerService(ItblCustomerRepository _tblCustomerRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblCustomerRepository = _tblCustomerRepository;
            this._UnitOfWork = _UnitOfWork;
        }
        private void Save()
        {
            _UnitOfWork.Commit();
        }
        /// <summary>
        /// get all card group
        /// </summary>
        /// <returns></returns>
        public IEnumerable<tblCustomer> GetAll()
        {
            var query = from n in _tblCustomerRepository.Table
                        select n;

            //if (!string.IsNullOrEmpty(AuthCustomerIds))
            //{
            //    var list = AuthCustomerIds.Split(';');
            //    query = query.Where(n => list.Contains(n.CustomerID.ToString()));
            //}

            return query;
        }



        public IEnumerable<tblCustomer_API3rd> GetTop10(string key)
        {
            var query = from c in _tblCustomerRepository.Table
                            //join p in _tblCustomerGroupRepository.Table on c.CustomerGroupID equals p.CustomerGroupID.ToString()
                        orderby (c.SortOrder)
                        select new tblCustomer_API3rd()
                        {
                            CustomerID = c.CustomerID.ToString(),
                            CustomerCode = c.CustomerCode,
                            CustomerGroupID = c.CustomerGroupID,
                            // CustomerGroupName = p.CustomerGroupName,
                            IDNumber = c.IDNumber,
                            Mobile = c.Mobile,
                            Address = c.Address,
                            CustomerName = c.CustomerName
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CustomerName.Contains(key) || n.CustomerCode.Contains(key) || n.IDNumber.Contains(key) || n.Mobile.Contains(key));
            }
            query = query.Take(10);

            return query;
        }

        public tblCustomer_API3rd GetByNameOrAdd(string name, string add)
        {
            var query = from c in _tblCustomerRepository.Table
                        where c.Inactive == false && c.CustomerName.Contains(name)
                        orderby (c.SortOrder)
                        select new tblCustomer_API3rd()
                        {
                            CustomerID = c.CustomerID.ToString(),
                            CustomerCode = c.CustomerCode,
                            CustomerGroupID = c.CustomerGroupID,
                            IDNumber = c.IDNumber,
                            Mobile = c.Mobile,
                            Address = c.Address,
                            CustomerName = c.CustomerName
                        };

            if (!String.IsNullOrWhiteSpace(add))
                query = query.Where(n => n.Address.Contains(add));
            return query.FirstOrDefault();
        }

        public MessageReport Create(tblCustomer obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCustomerRepository.Add(obj);

                Save();

                re.Message = "Tạo mới khách hàng thành công";
                re.isSuccess = true;
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public tblCustomer GetById(string id)
        {
            var query = from n in _tblCustomerRepository.Table
                        select n;
            var xxx = query.ToList();
            return query.ToList().Where(n => n.CustomerID.ToString().Contains(id.ToLower())).FirstOrDefault();
        }

        /// <summary>
        /// lấy theo sđt hoặc cmt
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="IdNumber"></param>
        /// <returns></returns>
        public tblCustomer_API3rd GetByMobileOrIdNumber(string mobile, string IdNumber)
        {

            if (String.IsNullOrWhiteSpace(IdNumber))
            {
                var query = from c in _tblCustomerRepository.Table
                            where c.Inactive == false &&  c.Mobile.Contains(mobile)
                            orderby (c.SortOrder)
                            select new tblCustomer_API3rd()
                            {
                                CustomerID = c.CustomerID.ToString(),
                                CustomerCode = c.CustomerCode,
                                CustomerGroupID = c.CustomerGroupID,
                                IDNumber = c.IDNumber,
                                Mobile = c.Mobile,
                                Address = c.Address,
                                CustomerName = c.CustomerName
                            };
                return query.FirstOrDefault();
            }
            else
            {
                var query = from c in _tblCustomerRepository.Table
                            where c.Inactive == false && (c.IDNumber.Contains(IdNumber) || c.Mobile.Contains(mobile))
                            orderby (c.SortOrder)
                            select new tblCustomer_API3rd()
                            {
                                CustomerID = c.CustomerID.ToString(),
                                CustomerCode = c.CustomerCode,
                                CustomerGroupID = c.CustomerGroupID,
                                IDNumber = c.IDNumber,
                                Mobile = c.Mobile,
                                Address = c.Address,
                                CustomerName = c.CustomerName
                            };
                return query.FirstOrDefault();
            }
        }

        public MessageReport Update(tblCustomer obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCustomerRepository.Update(obj);

                Save();

                re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["updateSuccess"];
                re.isSuccess = true;
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public MessageReport DeleteById(string id)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                var obj = GetById(id);
                if (obj != null)
                {
                    _tblCustomerRepository.Delete(n => n.CustomerID.ToString() == id);

                    Save();

                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["DeleteSuccess"];
                    re.isSuccess = true;
                }
                else
                {
                    re.Message = "Bản ghi không tồn tại";
                    re.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }


        IEnumerable<tblCustomer_API3rd> IAPI_CustomerService.GetByMobileOrName(string key)
        {
            var query = from c in _tblCustomerRepository.Table
                        where c.Inactive == false
                        orderby (c.SortOrder)
                        select new tblCustomer_API3rd()
                        {
                            CustomerID = c.CustomerID.ToString(),
                            CustomerCode = c.CustomerCode,
                            CustomerGroupID = c.CustomerGroupID,
                            IDNumber = c.IDNumber,
                            Mobile = c.Mobile,
                            Address = c.Address,
                            CustomerName = c.CustomerName
                        };
            if (!String.IsNullOrEmpty(key))
            {
                query = query.Where(n => n.CustomerName.Contains(key) || n.Mobile.Contains(key));
            }
            return query;
        }

        public IEnumerable<tblCustomer> GetAllCustomer(string key)
        {
            var query = from n in _tblCustomerRepository.Table
                        select n;

            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(n=>n.CustomerName.Contains(key));
            }

            return query;
        }
    }
}


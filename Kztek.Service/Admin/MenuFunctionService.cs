using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using Kztek.Model.Models;
using Kztek.Data.Repository;
using Kztek.Data.Infrastructure;
using Kztek.Data.SqlHelper;

namespace Kztek.Service.Admin
{
    public interface IMenuFunctionService
    {
        MenuFunction GetByName(string name);
        MenuFunction GetByName_Id(string name, string id);
        MenuFunction getById(string id);
        MenuFunction GetByControllerAction(string controller, string action);
        MenuFunction GetByController(string controller);
        IEnumerable<MenuFunction> GetAll();
        IEnumerable<MenuFunction> GetAllActive();
        IEnumerable<MenuFunction> GetAllParentActive();
        IEnumerable<MenuFunction> GetAllChildActive();
        IEnumerable<MenuFunction> GetAllChildByParentId(string id);
        IEnumerable<MenuFunction> GetAllActiveChildByParentId(string id);
        IPagedList<MenuFunction> GetAllParentPagingByFirst(string key, int PageNumber, int pageSize);
        List<MenuFunction> GetAllParentByFirst(string key);
        List<MenuFunction> GetAllMenu(string key, string groups = "");
        List<MenuFunction> GetAllMenuByPermisstion(string uId, bool isAdmin = false);
        List<MenuFunction> GetAllMenuBySuperAdmin();

        // lay toan bo menu theo quyen cua user hien tai
        bool Create(MenuFunction obj);
        bool Update(MenuFunction obj);
        bool DeleteById(string id);
        bool DeleteByIds(string lstId);
        bool ActiveByIds(string lstId, string active);

        //HNG
        /// <summary>
        /// Hàm cập nhập lại BreadCrumb cho menu không dùng trigger
        /// Cho thêm, sửa, xóa menu
        /// </summary>
        /// <param name="mId">MenuFunctionId</param>
        /// <param name="parentId"></param>
        /// <param name="typ">Dùng cho thêm hay sửa hay xóa: add|update|del</param>
        /// <returns></returns>
        bool UpdateBreadCrumb(MenuFunction model, string typ);

        IEnumerable<MenuFunction> GetAllParentActiveByGroupId(string gId);

        IEnumerable<MenuFunction> GetAllChildActiveByParentId(string id, string gId = "");
    }
    public class MenuFunctionService : IMenuFunctionService
    {
        private readonly IMenuFunctionRepository _MenuFunctionRepository;
        IRoleMenuRepository roleMenuRepository;
        IUserRoleRepository userRoleRepository;
        private readonly IUnitOfWork unitOfWork;
        private IMenuFunctionConfigService _MenuFunctionConfigService;

        public MenuFunctionService(IMenuFunctionRepository _MenuFunctionRepository, IRoleMenuRepository roleMenuRepository, IUserRoleRepository userRoleRepository, IUnitOfWork unitOfWork, IMenuFunctionConfigService _MenuFunctionConfigService)
        {
            this._MenuFunctionRepository = _MenuFunctionRepository;
            this.roleMenuRepository = roleMenuRepository;
            this.userRoleRepository = userRoleRepository;
            this.unitOfWork = unitOfWork;
            this._MenuFunctionConfigService = _MenuFunctionConfigService;
        }

        public bool Create(MenuFunction obj)
        {
            bool isSuccess = false;
            try
            {
                _MenuFunctionRepository.Add(obj);
                Save();
                isSuccess = true;

                //Update BreadCrumb
                UpdateBreadCrumb(obj, "add");
            }
            catch (Exception ex)
            {
                isSuccess = false;
                throw ex;
            }
            return isSuccess;
        }

        public bool DeleteById(string id)
        {
            bool isSuccess = false;
            var objDelete = _MenuFunctionRepository.GetById(id);
            if (objDelete != null)
            {
                if (objDelete.Active)
                {
                    isSuccess = false;
                }
                else
                {
                    _MenuFunctionRepository.Delete(objDelete);
                    Save();

                    //Update BreadCrumb
                    UpdateBreadCrumb(objDelete, "add");
                }
            }
            return isSuccess;
        }

        public bool DeleteByIds(string lstId)
        {
            bool isSuccess = false;
            try
            {
                string[] ids = lstId.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var id in ids)
                {
                    var objDelete = (from m in _MenuFunctionRepository.Table
                                     where m.Breadcrumb.Contains(id)
                                     select m).ToList();
                    if (objDelete.Any())
                    {
                        foreach (var item in objDelete)
                        {
                            //if (!item.Active)
                            //{
                                
                            //}
                            //_MenuFunctionRepository.Delete(item);
                            //Save();

                            var str = "Delete from MenuFunction Where Id = " + item.Id;
                            SqlExQuery<MenuFunction>.ExcuteNone(str);

                            //Update BreadCrumb
                            UpdateBreadCrumb(item, "add"); 
                        }

                    }
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                isSuccess = false;
            }
            return isSuccess;
        }

        public IEnumerable<MenuFunction> GetAll()
        {
            var query = from n in _MenuFunctionRepository.Table
                        where !n.Deleted
                        select n;
            return query;
        }

        public IEnumerable<MenuFunction> GetAllChildActive()
        {
            var query = from n in _MenuFunctionRepository.Table
                        where !n.Deleted && n.Active && n.ParentId != "0"
                        select n;
            return query;
        }

        public IPagedList<MenuFunction> GetAllParentPagingByFirst(string key, int PageNumber, int PageSize)
        {
            var query = from n in _MenuFunctionRepository.Table
                        where n.ParentId == "0" && !n.Deleted
                        select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.MenuName.Equals(key));
            }
            var list = new PagedList<MenuFunction>(query.OrderBy(n => n.OrderNumber), PageNumber, PageSize);
            return list;
        }

        public bool Update(MenuFunction obj)
        {
            bool isSuccess = false;
            try
            {
                _MenuFunctionRepository.Update(obj);
                Save();
                isSuccess = true;

                //Update BreadCrumb
                UpdateBreadCrumb(obj, "update");
            }
            catch (Exception ex)
            {
                isSuccess = false;
                throw ex;
            }
            return isSuccess;
        }

        public bool ActiveByIds(string lstId, string active)
        {
            bool isSuccess = true;
            try
            {
                string[] ids = lstId.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var id in ids)
                {
                    var obj = _MenuFunctionRepository.GetById(id);
                    if (obj != null)
                    {
                        if (active.Equals("True"))
                        {
                            obj.Active = true;
                        }
                        if (active.Equals("False"))
                        {
                            obj.Active = false;
                        }
                        _MenuFunctionRepository.Update(obj);
                        Save();
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                isSuccess = false;
            }
            return isSuccess;
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<MenuFunction> GetAllActive()
        {
            var query = from n in _MenuFunctionRepository.Table
                        where !n.Deleted && n.Active
                        orderby n.OrderNumber ascending
                        select n;
            return query;
        }

        public IEnumerable<MenuFunction> GetAllChildByParentId(string id)
        {
            var query = from n in _MenuFunctionRepository.Table
                        where !n.Deleted && n.ParentId.Equals(id)
                        orderby n.OrderNumber ascending
                        select n;
            return query;
        }

        public MenuFunction GetByName(string name)
        {
            var query = from n in _MenuFunctionRepository.Table
                        where !n.Deleted
                        select n;
            return query.FirstOrDefault(n => n.MenuName.Equals(name) && n.ActionName.Equals("Index"));
        }

        public MenuFunction GetByName_Id(string name, string id)
        {
            var query = from n in _MenuFunctionRepository.Table
                        where !n.Deleted
                        select n;
            return query.FirstOrDefault(n => n.MenuName.Equals(name) && n.ActionName.Equals("Index") && n.Id != id);
        }

        public MenuFunction getById(string id)
        {
            return _MenuFunctionRepository.GetById(id);
        }

        public List<MenuFunction> GetAllParentByFirst(string key)
        {
            var query = from n in _MenuFunctionRepository.Table
                        where n.ParentId == "0" && !n.Deleted
                        select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.MenuName.Contains(key));
            }

            return query.ToList();
        }

        public List<MenuFunction> GetAllMenu(string key, string groups = "")
        {
            var query = from n in _MenuFunctionRepository.Table
                        where !n.Deleted && n.MenuGroupListId.Contains(groups)

                        select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.MenuName.Contains(key));
            }

            return query.OrderBy(c => c.OrderNumber).ToList();
        }

        public MenuFunction GetByControllerAction(string controller, string action)
        {
            var query = from n in _MenuFunctionRepository.Table
                        where !n.Deleted
                        select n;
            return query.FirstOrDefault(n => n.ControllerName.Equals(controller) && n.ActionName.Equals(action));
        }

        public MenuFunction GetByController(string controller)
        {
            var query = from n in _MenuFunctionRepository.Table
                        where !n.Deleted
                        select n;
            return query.FirstOrDefault(n => n.ControllerName.Equals(controller));
        }

        public IEnumerable<MenuFunction> GetAllParentActive()
        {
            var query = from n in _MenuFunctionRepository.Table
                        where n.ParentId == "0" && !n.Deleted && n.Active
                        select n;
            return query;
        }

        public IEnumerable<MenuFunction> GetAllActiveChildByParentId(string id)
        {
            var query = from n in _MenuFunctionRepository.Table
                        where !n.Deleted && n.ParentId.Equals(id) && n.Active
                        orderby n.OrderNumber ascending
                        select n;
            return query;
        }

        public List<MenuFunction> GetAllMenuByPermisstion(string uId, bool isAdmin = false)
        {
            var query = from m in _MenuFunctionRepository.Table
                        join rm in roleMenuRepository.Table on m.Id equals rm.MenuId into rMenu
                        from rm in rMenu.DefaultIfEmpty()
                        join ur in userRoleRepository.Table on rm.RoleId equals ur.RoleId into uRole
                        from ur in uRole.DefaultIfEmpty()
                        where ur.UserId == uId && m.Active && !m.Deleted /*&& m.MenuType == "1"*/
                        orderby m.OrderNumber
                        select m;

            if (isAdmin)
            {
                query = from m in _MenuFunctionRepository.Table
                        join rm in roleMenuRepository.Table on m.Id equals rm.MenuId into rMenu
                        from rm in rMenu.DefaultIfEmpty()
                        where m.Active && !m.Deleted /*&& m.MenuType == "1"*/
                        orderby m.OrderNumber
                        select m;
            }

            var listShow = _MenuFunctionConfigService.GetAll();
            if (listShow.Any())
            {
                var lID = new List<string>();

                foreach (var item in listShow)
                {
                    lID.Add(item.MenuFunctionId);
                }

                query = query.Where(n => lID.Contains(n.Id));
            }

            return query.ToList();
        }


        public bool UpdateBreadCrumb(MenuFunction model, string typ)
        {
            var chk = true;
            try
            {
                //Chỉ update khi menu có tồn tại
                //var model = _MenuFunctionRepository.GetById(mId);
                if (model != null)
                {
                    if (typ == "add") // trường hợp tạo mới menu
                    {
                        //Nếu ParentId==0 thì cập nhập luôn BreadCrumb là /{MenuId}/
                        if (model.ParentId == "0")
                        {
                            model.Breadcrumb = string.Format("/{0}/", model.Id);
                            model.Dept = 1;
                        }
                        else
                        {
                            // dùng đệ quy truy vấn ngược lên từ ParentId hiện tại để lấy hết các Menu parent và lưu vào trong 1 List tuần tự sau này xử lý ngược lại
                            // kết quả của truy vấn theo thứ tự từ cháu lên tới cha, ông, cụ, kỵ
                            var b = ListBreadCrumb(model);
                            model.Breadcrumb = b.Breadcrumb;
                            model.Dept = b.Dept;
                        }

                        // #Save
                        _MenuFunctionRepository.Update(model);
                        Save();
                    }
                    else if (typ == "update") // trường hợp cập nhập menu
                    {
                        // check ParentId
                        if (model.ParentId == "0")
                        {
                            model.Breadcrumb = string.Format("/{0}/", model.Id);
                            model.Dept = 1;
                        }
                        else
                        {
                            //remove bỏ những breadCrumb cũ
                            var oldBreadCrumbs = (from m in _MenuFunctionRepository.Table
                                                  where m.Breadcrumb.Contains("/" + model.Id + "/")
                                                  select m).ToList();

                            foreach (var item in oldBreadCrumbs)
                            {
                                item.Breadcrumb.Replace(model.ParentId + "/", "");
                                _MenuFunctionRepository.Update(item);
                            }
                            Save();

                            var b = ListBreadCrumb(model);
                            model.Breadcrumb = b.Breadcrumb;
                            model.Dept = b.Dept;
                        }

                        // #Save new BreadCrumb
                        _MenuFunctionRepository.Update(model);
                        Save();
                    }
                    else if (typ == "del") // trường hợp xóa menu
                    {
                        //remove bỏ những breadCrumb cũ
                        var oldBreadCrumbs = from m in _MenuFunctionRepository.Table
                                             where m.Breadcrumb.Contains("/" + model.ParentId + "/")
                                             select m;

                        foreach (var item in oldBreadCrumbs)
                        {
                            item.Breadcrumb.Replace(model.ParentId + "/", "");
                            _MenuFunctionRepository.Update(item);
                        }
                        Save();
                    }
                }

            }
            catch (Exception ex)
            {
                chk = false;
            }
            return chk;
        }

        //Hàm trả về string BreadCrumb khi đã tập hợp đc các menu cha bắt đầu từ menu truyền vào
        public MenuFunction ListBreadCrumb(MenuFunction model)
        {
            var listb = new List<MenuFunction>();
            listb.Add(model); // add cấp nhỏ nhất
            DataBreadCrumbs(model, listb);
            var b = "/";
            if (listb.Any())
            {
                for (int i = listb.Count; i > 0; i--)
                {
                    var m = listb[i - 1];
                    b += m.Id + "/";
                }
            }
            //dùng new MenuFunction làm nơi chứa tạm dữ liệu tìm đc
            return new MenuFunction { Breadcrumb = b, Dept = listb.Count };
        }

        public void DataBreadCrumbs(MenuFunction model, List<MenuFunction> listBreadCrumb)
        {
            //Truy vấn ngược dần lên các cấp menu cha rồi đưa vào một tập hợp
            var parentMenu = _MenuFunctionRepository.GetById(model.ParentId);
            if (parentMenu != null)
            {
                listBreadCrumb.Add(parentMenu);
                // Gọi lại cho đến hết (parentId==0 là hết)
                DataBreadCrumbs(parentMenu, listBreadCrumb);
            }
        }

        public IEnumerable<MenuFunction> GetAllParentActiveByGroupId(string gId)
        {
            var model = from n in _MenuFunctionRepository.Table
                        where n.ParentId == "0" && n.Active && n.MenuGroupListId.Contains(gId)
                        select n;
            
            return model;
        }

        public IEnumerable<MenuFunction> GetAllChildActiveByParentId(string id, string gId = "")
        {
            var model = from n in _MenuFunctionRepository.Table
                        where n.ParentId == id && n.Active && n.MenuGroupListId.Contains(gId)
                        select n;

            return model;
        }

        public List<MenuFunction> GetAllMenuBySuperAdmin()
        {
            var query = from n in _MenuFunctionRepository.Table
                        select n;

            return query.ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.D3Shop.Admin.Shared.Requests.MenuRequests
{
    /// <summary>
    /// 设置可见性请求
    /// </summary>
    public class SetVisibilityRequest
    {
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisible { get; set; }
    }
}

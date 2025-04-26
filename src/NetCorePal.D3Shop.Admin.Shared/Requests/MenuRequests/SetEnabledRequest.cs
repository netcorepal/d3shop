using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.D3Shop.Admin.Shared.Requests.MenuRequests
{
    /// <summary>
    /// 设置启用状态请求
    /// </summary>
    public class SetEnabledRequest
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}

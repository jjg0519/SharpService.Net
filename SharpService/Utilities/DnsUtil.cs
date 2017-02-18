﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SharpService.Utilities
{
    public static class DnsUtil
    {

        public static async Task<string> GetIpAddressAsync(bool ipv4 = true)
        {
            var hostEntry = await Dns.GetHostEntryAsync(Dns.GetHostName());
            IPAddress ipaddress = null;
            if (ipv4)
            {
                ipaddress = (from ip in hostEntry.AddressList
                             where
                             (!IPAddress.IsLoopback(ip) && ip.AddressFamily == AddressFamily.InterNetwork)
                             select ip)
                             .FirstOrDefault();
            }
            else
            {
                ipaddress = (from ip in hostEntry.AddressList
                             where
                             (!IPAddress.IsLoopback(ip) && ip.AddressFamily == AddressFamily.InterNetworkV6)
                             select ip)
                             .FirstOrDefault();
            }
            if (ipaddress != null)
            {
                return ipaddress.ToString();
            }
            return string.Empty;
        }
    }
}

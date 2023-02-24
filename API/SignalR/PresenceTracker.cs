using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Localization;

namespace API.SignalR
{
    public class PresenceTracker
    {
        // Dictionary<string, List<string>> 
        // username, list of connection IDs for particalar user.
        // Usually one ID, but in some situation, the same user can be logged from the laptop, phone, tablet etc.
        // So, that is a reason why Lis of IDs.
        private static readonly Dictionary<string, List<string>> OnlineUsers 
            = new Dictionary<string, List<string>>();

        public Task<bool> UserConnected(string username, string connectionId)
        {
            // Dictionary is not safe thread type of object, 
            // If multiple concurent(at the same time) users want to reach Dictionary, it can be a problem.
            // So , we use lock. lock onlineuser. Dictionary will be lock for other users, 
            // while the current user is being added to the dictionary as onlineUser. In this way, no colisions can happen. 
            bool isOnline = false;
            lock(OnlineUsers)
            {
                if(OnlineUsers.ContainsKey(username))
                {
                    OnlineUsers[username].Add(connectionId);
                }
                else
                {
                    OnlineUsers.Add(username, new List<string>{connectionId});
                    isOnline = true;
                }
            }

            return Task.FromResult(isOnline);
        }

        public Task<bool> UserDisconnected(string username, string connectionId)
        {
            bool isOffline = false;
            lock(OnlineUsers)
            {
                if (!OnlineUsers.ContainsKey(username)) return Task.FromResult(isOffline);
                
                OnlineUsers[username].Remove(connectionId);

                if(OnlineUsers[username].Count == 0)
                {
                    OnlineUsers.Remove(username);
                    isOffline = true;
                } 
            }

            return Task.FromResult(isOffline);
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onLineUsers;
            lock(OnlineUsers)
            {
                onLineUsers = OnlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }

            return Task.FromResult(onLineUsers);
        }

        public static Task<List<string>> GetConnectionForUser(string username)
        {
            List<string> connectionIds;

            lock(OnlineUsers)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(username);
            }

            return Task.FromResult(connectionIds);
            
        }
    }
}
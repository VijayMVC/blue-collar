﻿//-----------------------------------------------------------------------
// <copyright file="DeleteScheduleHandler.cs" company="Tasty Codes">
//     Copyright (c) 2011 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------

namespace BlueCollar.Dashboard
{
    using System;
    using System.Web;

    /// <summary>
    /// Implements <see cref="IDashboardHandler"/> to delete a schedule.
    /// </summary>
    public sealed class DeleteScheduleHandler : DashboardHandlerBase
    {
        private long? id;

        /// <summary>
        /// Initializes a new instance of the DeleteScheduleHandler class.
        /// </summary>
        /// <param name="repositoryFactory">The repository factory to use.</param>
        public DeleteScheduleHandler(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
        }

        /// <summary>
        /// Gets the ID of the resource requesting to be deleted.
        /// </summary>
        public long Id
        {
            get
            {
                if (this.id == null)
                {
                    this.id = Helper.RouteIntValue(0) ?? 0;
                }

                return this.id.Value;
            }
        }

        /// <summary>
        /// Gets the cache modes available for responses generated by this instance.
        /// </summary>
        protected override ResponseCacheModes CacheModes
        {
            get { return ResponseCacheModes.None; }
        }

        /// <summary>
        /// Performs the concrete request operation and returns the output
        /// as a byte array.
        /// </summary>
        /// <param name="context">The HTTP context to perform the request for.</param>
        /// <returns>The response to write.</returns>
        protected override byte[] PerformRequest(HttpContextBase context)
        {
            if (this.Id > 0)
            {
                this.Repository.DeleteSchedule(this.Id, null);
            }
            else
            {
                BadRequest();
            }

            return null;
        }
    }
}
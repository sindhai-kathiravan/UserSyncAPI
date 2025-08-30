using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web.Http;
using UserSyncApi.Authentication;
using UserSyncApi.Helpers;
using UserSyncApi.Models;

namespace UserSyncApi.Controllers
{
    [BasicAuthentication]
    public class UsersController : ApiController
    {
        [HttpPost]
        [Route("api/users/create")]
        public IHttpActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                using (var scope = new TransactionScope())
                {

                    foreach (var dbKey in request.TargetDatabases)
                    {

                        using (SqlConnection conn = DbConnectionFactory.GetConnection(dbKey))
                        {
                            conn.Open();

                            string sql = @"
                INSERT INTO Users (
                    user_name, user_loginname, user_loggedonat, user_fullname, user_email,
                    user_initials, user_password, user_department, user_loggedin, user_inmodule,
                    g2version, lastlogin, os, clr, user_menus, logincount, libraries_readonly,
                    group_company, screen1_res, screen2_res, screen3_res, screen4_res,
                    po_auth_id, po_auth_all, order_alerts, po_auth_temp_user_id, maxordervalue,
                    outofoffice, default_order_department, porole_id, deleted, alias_username_1,
                    invoicebarcodeprinter, smtpserver, factory_id, piecemonitoringaccesslevel,
                    exclassedit, timesheetsaccesslevel, initial_windows, fabscheduleaccesslevel,
                    fablinescheduleaccesslevel, paintlinescheduleaccesslevel, contractsaccesslevel,
                    g2updaterversion, updatelocation_id, allocationadmin, user_password_last_changed,
                    date_created, createdbyuser_id, date_modified, modifiedbyuser_id,
                    loggedinoncomputer, yloc, ylocdsc, locked, fattempt, remarks, releasedt,
                    releaseby, inactive, inactiveremarks, inactivereleasedt, inactivereleaseby,
                    password_attempts, password_updated_flag, unlock_date
                )
                VALUES (
                    @UserName, @UserLoginName, @UserLoggedOnAt, @UserFullName, @UserEmail,
                    @UserInitials, @UserPassword, @UserDepartment, @UserLoggedIn, @UserInModule,
                    @G2Version, @LastLogin, @OS, @CLR, @UserMenus, @LoginCount, @LibrariesReadOnly,
                    @GroupCompany, @Screen1Res, @Screen2Res, @Screen3Res, @Screen4Res,
                    @POAuthId, @POAuthAll, @OrderAlerts, @POAuthTempUserId, @MaxOrderValue,
                    @OutOfOffice, @DefaultOrderDepartment, @PORoleId, @Deleted, @AliasUserName1,
                    @InvoiceBarcodePrinter, @SmtpServer, @FactoryId, @PieceMonitoringAccessLevel,
                    @ExClassEdit, @TimesheetsAccessLevel, @InitialWindows, @FabScheduleAccessLevel,
                    @FabLineScheduleAccessLevel, @PaintLineScheduleAccessLevel, @ContractsAccessLevel,
                    @G2UpdaterVersion, @UpdateLocationId, @AllocationAdmin, @UserPasswordLastChanged,
                    GETDATE(), @CreatedByUserId, GETDATE(), @ModifiedByUserId,
                    @LoggedInOnComputer, @Yloc, @YlocDsc, @Locked, @FAttempt, @Remarks, @ReleaseDt,
                    @ReleaseBy, @Inactive, @InactiveRemarks, @InactiveReleaseDt, @InactiveReleaseBy,
                    @PasswordAttempts, @PasswordUpdatedFlag, @UnlockDate
                );
                SELECT SCOPE_IDENTITY();";

                            using (SqlCommand cmd = new SqlCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@UserName", request.UserName);
                                cmd.Parameters.AddWithValue("@UserLoginName", request.UserLoginName);
                                cmd.Parameters.AddWithValue("@UserLoggedOnAt", request.UserLoggedOnAt);
                                cmd.Parameters.AddWithValue("@UserFullName", request.UserFullName);
                                cmd.Parameters.AddWithValue("@UserEmail", request.UserEmail);
                                cmd.Parameters.AddWithValue("@UserInitials", request.UserInitials);
                                cmd.Parameters.AddWithValue("@UserPassword", request.UserPassword);
                                cmd.Parameters.AddWithValue("@UserDepartment", request.UserDepartment);
                                cmd.Parameters.AddWithValue("@UserLoggedIn", request.UserLoggedIn);
                                cmd.Parameters.AddWithValue("@UserInModule", request.UserInModule);
                                cmd.Parameters.AddWithValue("@G2Version", (object)request.G2Version ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@LastLogin", (object)request.LastLogin ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@OS", request.OS);
                                cmd.Parameters.AddWithValue("@CLR", request.CLR);
                                cmd.Parameters.AddWithValue("@UserMenus", request.UserMenus);
                                cmd.Parameters.AddWithValue("@LoginCount", request.LoginCount);
                                cmd.Parameters.AddWithValue("@LibrariesReadOnly", request.LibrariesReadOnly);
                                cmd.Parameters.AddWithValue("@GroupCompany", request.GroupCompany);
                                cmd.Parameters.AddWithValue("@Screen1Res", request.Screen1Res);
                                cmd.Parameters.AddWithValue("@Screen2Res", request.Screen2Res);
                                cmd.Parameters.AddWithValue("@Screen3Res", request.Screen3Res);
                                cmd.Parameters.AddWithValue("@Screen4Res", request.Screen4Res);
                                cmd.Parameters.AddWithValue("@POAuthId", request.POAuthId);
                                cmd.Parameters.AddWithValue("@POAuthAll", request.POAuthAll);
                                cmd.Parameters.AddWithValue("@OrderAlerts", request.OrderAlerts);
                                cmd.Parameters.AddWithValue("@POAuthTempUserId", request.POAuthTempUserId);
                                cmd.Parameters.AddWithValue("@MaxOrderValue", request.MaxOrderValue);
                                cmd.Parameters.AddWithValue("@OutOfOffice", request.OutOfOffice);
                                cmd.Parameters.AddWithValue("@DefaultOrderDepartment", request.DefaultOrderDepartment);
                                cmd.Parameters.AddWithValue("@PORoleId", request.PORoleId);
                                cmd.Parameters.AddWithValue("@Deleted", request.Deleted);
                                cmd.Parameters.AddWithValue("@AliasUserName1", (object)request.AliasUserName1 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@InvoiceBarcodePrinter", request.InvoiceBarcodePrinter);
                                cmd.Parameters.AddWithValue("@SmtpServer", request.SmtpServer);
                                cmd.Parameters.AddWithValue("@FactoryId", request.FactoryId);
                                cmd.Parameters.AddWithValue("@PieceMonitoringAccessLevel", request.PieceMonitoringAccessLevel);
                                cmd.Parameters.AddWithValue("@ExClassEdit", request.ExClassEdit);
                                cmd.Parameters.AddWithValue("@TimesheetsAccessLevel", request.TimesheetsAccessLevel);
                                cmd.Parameters.AddWithValue("@InitialWindows", request.InitialWindows);
                                cmd.Parameters.AddWithValue("@FabScheduleAccessLevel", request.FabScheduleAccessLevel);
                                cmd.Parameters.AddWithValue("@FabLineScheduleAccessLevel", request.FabLineScheduleAccessLevel);
                                cmd.Parameters.AddWithValue("@PaintLineScheduleAccessLevel", request.PaintLineScheduleAccessLevel);
                                cmd.Parameters.AddWithValue("@ContractsAccessLevel", request.ContractsAccessLevel);
                                cmd.Parameters.AddWithValue("@G2UpdaterVersion", (object)request.G2UpdaterVersion ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@UpdateLocationId", request.UpdateLocationId);
                                cmd.Parameters.AddWithValue("@AllocationAdmin", request.AllocationAdmin);
                                cmd.Parameters.AddWithValue("@UserPasswordLastChanged", (object)request.UserPasswordLastChanged ?? DBNull.Value);
                                //cmd.Parameters.AddWithValue("@DateCreated", (object)request.DateCreated ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@CreatedByUserId", (object)request.CreatedByUserId ?? DBNull.Value);
                                //cmd.Parameters.AddWithValue("@DateModified", (object)request.DateModified ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ModifiedByUserId", (object)request.ModifiedByUserId ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@LoggedInOnComputer", request.LoggedInOnComputer);
                                cmd.Parameters.AddWithValue("@Yloc", (object)request.YLoc ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@YlocDsc", (object)request.YLocDsc ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@Locked", (object)request.Locked ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@FAttempt", (object)request.FAttempt ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@Remarks", (object)request.Remarks ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ReleaseDt", (object)request.ReleaseDt ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ReleaseBy", (object)request.ReleaseBy ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@Inactive", (object)request.Inactive ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@InactiveRemarks", (object)request.InactiveRemarks ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@InactiveReleaseDt", (object)request.InactiveReleaseDt ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@InactiveReleaseBy", (object)request.InactiveReleaseBy ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@PasswordAttempts", (object)request.PasswordAttempts ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@PasswordUpdatedFlag", (object)request.PasswordUpdatedFlag ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@UnlockDate", (object)request.UnlockDate ?? DBNull.Value);

                                var newId = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                        }
                    }
                }
                return Content(HttpStatusCode.OK, new { Message = Common.Constants.Messages.USER_CREATED_SUCCESSFULLY });
            }
            catch (Exception ex)
            {
                Logger.Log($"Error in CreateUser: {ex.Message}");
                Logger.Log($"StackTrace in CreateUser: { ex.StackTrace.ToString()}");
                return Content(HttpStatusCode.InternalServerError, Common.Constants.Messages.AN_UNEXPECTED_ERROR_OCCURRED);
            }
        }

        [HttpGet]
        [Route("api/users/get")]
        public IHttpActionResult GetUser(int UserId)
        {
            User userObj = null;
            try
            {

                using (SqlConnection conn = DbConnectionFactory.GetDefaultConnection())
                {
                    conn.Open();
                    string query = @"
                SELECT user_id, user_name, user_loginname, user_loggedonat, user_fullname, user_email,
                       user_initials, user_password, user_department, user_loggedin, user_inmodule,
                       g2version, lastlogin, os, clr, user_menus, logincount, libraries_readonly,
                       group_company, screen1_res, screen2_res, screen3_res, screen4_res,
                       po_auth_id, po_auth_all, order_alerts, po_auth_temp_user_id, maxordervalue,
                       outofoffice, default_order_department, porole_id, deleted, alias_username_1,
                       invoicebarcodeprinter, smtpserver, factory_id, piecemonitoringaccesslevel,
                       exclassedit, timesheetsaccesslevel, initial_windows, fabscheduleaccesslevel,
                       fablinescheduleaccesslevel, paintlinescheduleaccesslevel, contractsaccesslevel,
                       g2updaterversion, updatelocation_id, allocationadmin, user_password_last_changed,
                       date_created, createdbyuser_id, date_modified, modifiedbyuser_id,
                       loggedinoncomputer, yloc, ylocdsc, locked, fattempt, remarks, releasedt,
                       releaseby, inactive, inactiveremarks, inactivereleasedt, inactivereleaseby,
                       password_attempts, password_updated_flag, unlock_date
                FROM Users
                WHERE deleted = 0 and user_id = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userObj = new User
                                {
                                    UserId = reader["user_id"] != DBNull.Value ? Convert.ToInt32(reader["user_id"]) : 0,
                                    UserName = reader["user_name"] != DBNull.Value ? Convert.ToString(reader["user_name"]) : null,
                                    UserLoginName = reader["user_loginname"] != DBNull.Value ? reader["user_loginname"].ToString() : null,
                                    UserLoggedOnAt = reader["user_loggedonat"] != DBNull.Value ? Convert.ToString(reader["user_loggedonat"]) : null,
                                    UserFullName = reader["user_fullname"] != DBNull.Value ? reader["user_fullname"].ToString() : null,
                                    UserEmail = reader["user_email"] != DBNull.Value ? reader["user_email"].ToString() : null,
                                    UserInitials = reader["user_initials"] != DBNull.Value ? reader["user_initials"].ToString() : null,
                                    UserPassword = reader["user_password"] != DBNull.Value ? reader["user_password"].ToString() : null,
                                    UserDepartment = reader["user_department"] != DBNull.Value ? Convert.ToInt32(reader["user_department"]) : 0,
                                    UserLoggedIn = reader["user_loggedin"] != DBNull.Value ? Convert.ToBoolean(reader["user_loggedin"]) : false,
                                    UserInModule = reader["user_inmodule"] != DBNull.Value ? Convert.ToInt32(reader["user_inmodule"]) : 0,
                                    G2Version = reader["g2version"] != DBNull.Value ? reader["g2version"].ToString() : null,
                                    LastLogin = reader["lastlogin"] != DBNull.Value ? Convert.ToDateTime(reader["lastlogin"]) : (DateTime?)null,
                                    OS = reader["os"] != DBNull.Value ? reader["os"].ToString() : null,
                                    CLR = reader["clr"] != DBNull.Value ? reader["clr"].ToString() : null,
                                    UserMenus = reader["user_menus"] != DBNull.Value ? reader["user_menus"].ToString() : null,
                                    LoginCount = reader["logincount"] != DBNull.Value ? Convert.ToInt32(reader["logincount"]) : 0,
                                    LibrariesReadOnly = reader["libraries_readonly"] != DBNull.Value ? Convert.ToBoolean(reader["libraries_readonly"]) : false,
                                    GroupCompany = reader["group_company"] != DBNull.Value ? Convert.ToInt32(reader["group_company"]) : 0,
                                    Screen1Res = reader["screen1_res"] != DBNull.Value ? reader["screen1_res"].ToString() : null,
                                    Screen2Res = reader["screen2_res"] != DBNull.Value ? reader["screen2_res"].ToString() : null,
                                    Screen3Res = reader["screen3_res"] != DBNull.Value ? reader["screen3_res"].ToString() : null,
                                    Screen4Res = reader["screen4_res"] != DBNull.Value ? reader["screen4_res"].ToString() : null,
                                    POAuthId = reader["po_auth_id"] != DBNull.Value ? Convert.ToInt32(reader["po_auth_id"]) : 0,
                                    POAuthAll = reader["po_auth_all"] != DBNull.Value ? Convert.ToBoolean(reader["po_auth_all"]) : false,
                                    OrderAlerts = reader["order_alerts"] != DBNull.Value ? Convert.ToBoolean(reader["order_alerts"]) : false,
                                    POAuthTempUserId = reader["po_auth_temp_user_id"] != DBNull.Value ? Convert.ToInt32(reader["po_auth_temp_user_id"]) : 0,
                                    MaxOrderValue = reader["maxordervalue"] != DBNull.Value ? Convert.ToInt32(reader["maxordervalue"]) : 0,
                                    OutOfOffice = reader["outofoffice"] != DBNull.Value ? Convert.ToBoolean(reader["outofoffice"]) : false,
                                    DefaultOrderDepartment = reader["default_order_department"] != DBNull.Value ? Convert.ToInt32(reader["default_order_department"]) : 0,
                                    PORoleId = reader["porole_id"] != DBNull.Value ? Convert.ToInt32(reader["porole_id"]) : 0,
                                    Deleted = reader["deleted"] != DBNull.Value ? Convert.ToBoolean(reader["deleted"]) : false,
                                    AliasUserName1 = reader["alias_username_1"] != DBNull.Value ? reader["alias_username_1"].ToString() : null,
                                    InvoiceBarcodePrinter = reader["invoicebarcodeprinter"] != DBNull.Value ? reader["invoicebarcodeprinter"].ToString() : null,
                                    SmtpServer = reader["smtpserver"] != DBNull.Value ? reader["smtpserver"].ToString() : null,
                                    FactoryId = reader["factory_id"] != DBNull.Value ? Convert.ToInt32(reader["factory_id"]) : 0,
                                    PieceMonitoringAccessLevel = reader["piecemonitoringaccesslevel"] != DBNull.Value ? Convert.ToInt32(reader["piecemonitoringaccesslevel"]) : 0,
                                    ExClassEdit = reader["exclassedit"] != DBNull.Value ? Convert.ToInt32(reader["exclassedit"]) : 0,
                                    TimesheetsAccessLevel = reader["timesheetsaccesslevel"] != DBNull.Value ? Convert.ToInt32(reader["timesheetsaccesslevel"]) : 0,
                                    InitialWindows = reader["initial_windows"] != DBNull.Value ? reader["initial_windows"].ToString() : null,
                                    FabScheduleAccessLevel = reader["fabscheduleaccesslevel"] != DBNull.Value ? Convert.ToInt32(reader["fabscheduleaccesslevel"]) : 0,
                                    FabLineScheduleAccessLevel = reader["fablinescheduleaccesslevel"] != DBNull.Value ? Convert.ToInt32(reader["fablinescheduleaccesslevel"]) : 0,
                                    PaintLineScheduleAccessLevel = reader["paintlinescheduleaccesslevel"] != DBNull.Value ? Convert.ToInt32(reader["paintlinescheduleaccesslevel"]) : 0,
                                    ContractsAccessLevel = reader["contractsaccesslevel"] != DBNull.Value ? Convert.ToInt32(reader["contractsaccesslevel"]) : 0,
                                    G2UpdaterVersion = reader["g2updaterversion"] != DBNull.Value ? reader["g2updaterversion"].ToString() : null,
                                    UpdateLocationId = reader["updatelocation_id"] != DBNull.Value ? Convert.ToInt32(reader["updatelocation_id"]) : 0,
                                    AllocationAdmin = reader["allocationadmin"] != DBNull.Value ? Convert.ToBoolean(reader["allocationadmin"]) : false,
                                    UserPasswordLastChanged = reader["user_password_last_changed"] != DBNull.Value ? Convert.ToDateTime(reader["user_password_last_changed"]) : (DateTime?)null,
                                    DateCreated = reader["date_created"] != DBNull.Value ? Convert.ToDateTime(reader["date_created"]) : (DateTime?)null,
                                    CreatedByUserId = reader["createdbyuser_id"] != DBNull.Value ? Convert.ToInt32(reader["createdbyuser_id"]) : 0,
                                    DateModified = reader["date_modified"] != DBNull.Value ? Convert.ToDateTime(reader["date_modified"]) : (DateTime?)null,
                                    ModifiedByUserId = reader["modifiedbyuser_id"] != DBNull.Value ? Convert.ToInt32(reader["modifiedbyuser_id"]) : 0,
                                    LoggedInOnComputer = reader["loggedinoncomputer"] != DBNull.Value ? reader["loggedinoncomputer"].ToString() : null,
                                    YLoc = reader["yloc"] != DBNull.Value ? reader["yloc"].ToString() : null,
                                    YLocDsc = reader["ylocdsc"] != DBNull.Value ? reader["ylocdsc"].ToString() : null,
                                    Locked = reader["locked"] != DBNull.Value ? Convert.ToInt32(reader["locked"]) : 0,
                                    FAttempt = reader["fattempt"] != DBNull.Value ? Convert.ToInt32(reader["fattempt"]) : 0,
                                    Remarks = reader["remarks"] != DBNull.Value ? reader["remarks"].ToString() : null,
                                    ReleaseDt = reader["releasedt"] != DBNull.Value ? Convert.ToDateTime(reader["releasedt"]) : (DateTime?)null,
                                    ReleaseBy = reader["releaseby"] != DBNull.Value ? reader["releaseby"].ToString() : null,
                                    Inactive = reader["inactive"] != DBNull.Value ? Convert.ToInt32(reader["inactive"]) : 0,
                                    InactiveRemarks = reader["inactiveremarks"] != DBNull.Value ? reader["inactiveremarks"].ToString() : null,
                                    InactiveReleaseDt = reader["inactivereleasedt"] != DBNull.Value ? Convert.ToDateTime(reader["inactivereleasedt"]) : (DateTime?)null,
                                    InactiveReleaseBy = reader["inactivereleaseby"] != DBNull.Value ? reader["inactivereleaseby"].ToString() : null,
                                    PasswordAttempts = reader["password_attempts"] != DBNull.Value ? Convert.ToString(reader["password_attempts"]) : null,
                                    PasswordUpdatedFlag = reader["password_updated_flag"] != DBNull.Value ? Convert.ToString(reader["password_updated_flag"]) : null,
                                    UnlockDate = reader["unlock_date"] != DBNull.Value ? Convert.ToDateTime(reader["unlock_date"]) : (DateTime?)null
                                };
                                return Content(HttpStatusCode.OK, new { Message = Common.Constants.Messages.USER_RETRIEVED_SUCCESSFULLY, Data = new { User = userObj } });
                            }
                            else
                            {
                                return Content(HttpStatusCode.NotFound, new { Message = $"User with Id {UserId} was not found.", Error = Common.Constants.Errors.ERR_NOT_FOUND });
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error in GetUser: { ex.Message}");
                Logger.Log($"StackTrace in GetUser: { ex.StackTrace.ToString()}");
                return Content(HttpStatusCode.InternalServerError, new { Message = Common.Constants.Messages.AN_UNEXPECTED_ERROR_OCCURRED });
            }

        }

        [HttpGet]
        [Route("api/users/getAll")]
        public IHttpActionResult GetAllUser()
        {
            try
            {
                List<User> users = new List<User>();
                using (SqlConnection conn = DbConnectionFactory.GetDefaultConnection())
                {
                    conn.Open();
                    string query = @"
                            SELECT user_id, user_name, user_loginname, user_loggedonat, user_fullname, user_email,
                                   user_initials, user_password, user_department, user_loggedin, user_inmodule,
                                   g2version, lastlogin, os, clr, user_menus, logincount, libraries_readonly,
                                   group_company, screen1_res, screen2_res, screen3_res, screen4_res,
                                   po_auth_id, po_auth_all, order_alerts, po_auth_temp_user_id, maxordervalue,
                                   outofoffice, default_order_department, porole_id, deleted, alias_username_1,
                                   invoicebarcodeprinter, smtpserver, factory_id, piecemonitoringaccesslevel,
                                   exclassedit, timesheetsaccesslevel, initial_windows, fabscheduleaccesslevel,
                                   fablinescheduleaccesslevel, paintlinescheduleaccesslevel, contractsaccesslevel,
                                   g2updaterversion, updatelocation_id, allocationadmin, user_password_last_changed,
                                   date_created, createdbyuser_id, date_modified, modifiedbyuser_id,
                                   loggedinoncomputer, yloc, ylocdsc, locked, fattempt, remarks, releasedt,
                                   releaseby, inactive, inactiveremarks, inactivereleasedt, inactivereleaseby,
                                   password_attempts, password_updated_flag, unlock_date
                            FROM Users
                            WHERE deleted = 0";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new User
                                {
                                    UserId = reader["user_id"] != DBNull.Value ? Convert.ToInt32(reader["user_id"]) : 0,
                                    UserName = reader["user_name"] != DBNull.Value ? Convert.ToString(reader["user_name"]) : null,
                                    UserLoginName = reader["user_loginname"] != DBNull.Value ? reader["user_loginname"].ToString() : null,
                                    UserLoggedOnAt = reader["user_loggedonat"] != DBNull.Value ? Convert.ToString(reader["user_loggedonat"]) : null,
                                    UserFullName = reader["user_fullname"] != DBNull.Value ? reader["user_fullname"].ToString() : null,
                                    UserEmail = reader["user_email"] != DBNull.Value ? reader["user_email"].ToString() : null,
                                    UserInitials = reader["user_initials"] != DBNull.Value ? reader["user_initials"].ToString() : null,
                                    UserPassword = reader["user_password"] != DBNull.Value ? reader["user_password"].ToString() : null,
                                    UserDepartment = reader["user_department"] != DBNull.Value ? Convert.ToInt32(reader["user_department"]) : 0,
                                    UserLoggedIn = reader["user_loggedin"] != DBNull.Value ? Convert.ToBoolean(reader["user_loggedin"]) : false,
                                    UserInModule = reader["user_inmodule"] != DBNull.Value ? Convert.ToInt32(reader["user_inmodule"]) : 0,
                                    G2Version = reader["g2version"] != DBNull.Value ? reader["g2version"].ToString() : null,
                                    LastLogin = reader["lastlogin"] != DBNull.Value ? Convert.ToDateTime(reader["lastlogin"]) : (DateTime?)null,
                                    OS = reader["os"] != DBNull.Value ? reader["os"].ToString() : null,
                                    CLR = reader["clr"] != DBNull.Value ? reader["clr"].ToString() : null,
                                    UserMenus = reader["user_menus"] != DBNull.Value ? reader["user_menus"].ToString() : null,
                                    LoginCount = reader["logincount"] != DBNull.Value ? Convert.ToInt32(reader["logincount"]) : 0,
                                    LibrariesReadOnly = reader["libraries_readonly"] != DBNull.Value ? Convert.ToBoolean(reader["libraries_readonly"]) : false,
                                    GroupCompany = reader["group_company"] != DBNull.Value ? Convert.ToInt32(reader["group_company"]) : 0,
                                    Screen1Res = reader["screen1_res"] != DBNull.Value ? reader["screen1_res"].ToString() : null,
                                    Screen2Res = reader["screen2_res"] != DBNull.Value ? reader["screen2_res"].ToString() : null,
                                    Screen3Res = reader["screen3_res"] != DBNull.Value ? reader["screen3_res"].ToString() : null,
                                    Screen4Res = reader["screen4_res"] != DBNull.Value ? reader["screen4_res"].ToString() : null,
                                    POAuthId = reader["po_auth_id"] != DBNull.Value ? Convert.ToInt32(reader["po_auth_id"]) : 0,
                                    POAuthAll = reader["po_auth_all"] != DBNull.Value ? Convert.ToBoolean(reader["po_auth_all"]) : false,
                                    OrderAlerts = reader["order_alerts"] != DBNull.Value ? Convert.ToBoolean(reader["order_alerts"]) : false,
                                    POAuthTempUserId = reader["po_auth_temp_user_id"] != DBNull.Value ? Convert.ToInt32(reader["po_auth_temp_user_id"]) : 0,
                                    MaxOrderValue = reader["maxordervalue"] != DBNull.Value ? Convert.ToInt32(reader["maxordervalue"]) : 0,
                                    OutOfOffice = reader["outofoffice"] != DBNull.Value ? Convert.ToBoolean(reader["outofoffice"]) : false,
                                    DefaultOrderDepartment = reader["default_order_department"] != DBNull.Value ? Convert.ToInt32(reader["default_order_department"]) : 0,
                                    PORoleId = reader["porole_id"] != DBNull.Value ? Convert.ToInt32(reader["porole_id"]) : 0,
                                    Deleted = reader["deleted"] != DBNull.Value ? Convert.ToBoolean(reader["deleted"]) : false,
                                    AliasUserName1 = reader["alias_username_1"] != DBNull.Value ? reader["alias_username_1"].ToString() : null,
                                    InvoiceBarcodePrinter = reader["invoicebarcodeprinter"] != DBNull.Value ? reader["invoicebarcodeprinter"].ToString() : null,
                                    SmtpServer = reader["smtpserver"] != DBNull.Value ? reader["smtpserver"].ToString() : null,
                                    FactoryId = reader["factory_id"] != DBNull.Value ? Convert.ToInt32(reader["factory_id"]) : 0,
                                    PieceMonitoringAccessLevel = reader["piecemonitoringaccesslevel"] != DBNull.Value ? Convert.ToInt32(reader["piecemonitoringaccesslevel"]) : 0,
                                    ExClassEdit = reader["exclassedit"] != DBNull.Value ? Convert.ToInt32(reader["exclassedit"]) : 0,
                                    TimesheetsAccessLevel = reader["timesheetsaccesslevel"] != DBNull.Value ? Convert.ToInt32(reader["timesheetsaccesslevel"]) : 0,
                                    InitialWindows = reader["initial_windows"] != DBNull.Value ? reader["initial_windows"].ToString() : null,
                                    FabScheduleAccessLevel = reader["fabscheduleaccesslevel"] != DBNull.Value ? Convert.ToInt32(reader["fabscheduleaccesslevel"]) : 0,
                                    FabLineScheduleAccessLevel = reader["fablinescheduleaccesslevel"] != DBNull.Value ? Convert.ToInt32(reader["fablinescheduleaccesslevel"]) : 0,
                                    PaintLineScheduleAccessLevel = reader["paintlinescheduleaccesslevel"] != DBNull.Value ? Convert.ToInt32(reader["paintlinescheduleaccesslevel"]) : 0,
                                    ContractsAccessLevel = reader["contractsaccesslevel"] != DBNull.Value ? Convert.ToInt32(reader["contractsaccesslevel"]) : 0,
                                    G2UpdaterVersion = reader["g2updaterversion"] != DBNull.Value ? reader["g2updaterversion"].ToString() : null,
                                    UpdateLocationId = reader["updatelocation_id"] != DBNull.Value ? Convert.ToInt32(reader["updatelocation_id"]) : 0,
                                    AllocationAdmin = reader["allocationadmin"] != DBNull.Value ? Convert.ToBoolean(reader["allocationadmin"]) : false,
                                    UserPasswordLastChanged = reader["user_password_last_changed"] != DBNull.Value ? Convert.ToDateTime(reader["user_password_last_changed"]) : (DateTime?)null,
                                    DateCreated = reader["date_created"] != DBNull.Value ? Convert.ToDateTime(reader["date_created"]) : (DateTime?)null,
                                    CreatedByUserId = reader["createdbyuser_id"] != DBNull.Value ? Convert.ToInt32(reader["createdbyuser_id"]) : 0,
                                    DateModified = reader["date_modified"] != DBNull.Value ? Convert.ToDateTime(reader["date_modified"]) : (DateTime?)null,
                                    ModifiedByUserId = reader["modifiedbyuser_id"] != DBNull.Value ? Convert.ToInt32(reader["modifiedbyuser_id"]) : 0,
                                    LoggedInOnComputer = reader["loggedinoncomputer"] != DBNull.Value ? reader["loggedinoncomputer"].ToString() : null,
                                    YLoc = reader["yloc"] != DBNull.Value ? reader["yloc"].ToString() : null,
                                    YLocDsc = reader["ylocdsc"] != DBNull.Value ? reader["ylocdsc"].ToString() : null,
                                    Locked = reader["locked"] != DBNull.Value ? Convert.ToInt32(reader["locked"]) : 0,
                                    FAttempt = reader["fattempt"] != DBNull.Value ? Convert.ToInt32(reader["fattempt"]) : 0,
                                    Remarks = reader["remarks"] != DBNull.Value ? reader["remarks"].ToString() : null,
                                    ReleaseDt = reader["releasedt"] != DBNull.Value ? Convert.ToDateTime(reader["releasedt"]) : (DateTime?)null,
                                    ReleaseBy = reader["releaseby"] != DBNull.Value ? reader["releaseby"].ToString() : null,
                                    Inactive = reader["inactive"] != DBNull.Value ? Convert.ToInt32(reader["inactive"]) : 0,
                                    InactiveRemarks = reader["inactiveremarks"] != DBNull.Value ? reader["inactiveremarks"].ToString() : null,
                                    InactiveReleaseDt = reader["inactivereleasedt"] != DBNull.Value ? Convert.ToDateTime(reader["inactivereleasedt"]) : (DateTime?)null,
                                    InactiveReleaseBy = reader["inactivereleaseby"] != DBNull.Value ? reader["inactivereleaseby"].ToString() : null,
                                    PasswordAttempts = reader["password_attempts"] != DBNull.Value ? Convert.ToString(reader["password_attempts"]) : null,
                                    PasswordUpdatedFlag = reader["password_updated_flag"] != DBNull.Value ? Convert.ToString(reader["password_updated_flag"]) : null,
                                    UnlockDate = reader["unlock_date"] != DBNull.Value ? Convert.ToDateTime(reader["unlock_date"]) : (DateTime?)null
                                });
                            }
                        }
                    }
                }
                string message = users.Count > 0 ? $"Fetched {users.Count} users successfully." : "No users found.";
                return Content(HttpStatusCode.OK, new { Message = message, Data = new { UsersCount = users.Count, Users = users } });

            }
            catch (Exception ex)
            {
                Logger.Log($"Error in GetAllUser: { ex.Message}");
                Logger.Log($"StackTrace in GetAllUser: { ex.StackTrace.ToString()}");
                return Content(HttpStatusCode.InternalServerError, new { Message = Common.Constants.Messages.AN_UNEXPECTED_ERROR_OCCURRED });
            }
        }

        [HttpPut]
        [Route("api/users/update")]
        public IHttpActionResult UpdateUser([FromBody] UpdateUserRequest request)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var dbKey in request.TargetDatabases)
                    {
                        using (SqlConnection conn = DbConnectionFactory.GetConnection(dbKey))
                        {
                            conn.Open();
                            string sql = @"
                                        UPDATE Users SET
                                            user_name=@UserName,
                                            user_loginname=@UserLoginName,
                                            user_loggedonat=@UserLoggedOnAt,
                                            user_fullname=@UserFullName,
                                            user_email=@UserEmail,
                                            user_initials=@UserInitials,
                                            user_password=@UserPassword,
                                            user_department=@UserDepartment,
                                            user_loggedin=@UserLoggedIn,
                                            user_inmodule=@UserInModule,
                                            g2version=@G2Version,
                                            lastlogin=@LastLogin,
                                            os=@OS,
                                            clr=@CLR,
                                            user_menus=@UserMenus,
                                            logincount=@LoginCount,
                                            libraries_readonly=@LibrariesReadonly,
                                            group_company=@GroupCompany,
                                            screen1_res=@Screen1Res,
                                            screen2_res=@Screen2Res,
                                            screen3_res=@Screen3Res,
                                            screen4_res=@Screen4Res,
                                            po_auth_id=@PoAuthId,
                                            po_auth_all=@PoAuthAll,
                                            order_alerts=@OrderAlerts,
                                            po_auth_temp_user_id=@PoAuthTempUserId,
                                            maxordervalue=@MaxOrderValue,
                                            outofoffice=@OutOfOffice,
                                            default_order_department=@DefaultOrderDepartment,
                                            porole_id=@PoRoleId,
                                            deleted=@Deleted,
                                            alias_username_1=@AliasUserName1,
                                            invoicebarcodeprinter=@InvoiceBarcodePrinter,
                                            smtpserver=@SmtpServer,
                                            factory_id=@FactoryId,
                                            piecemonitoringaccesslevel=@PieceMonitoringAccessLevel,
                                            exclassedit=@ExClassEdit,
                                            timesheetsaccesslevel=@TimesheetsAccessLevel,
                                            initial_windows=@InitialWindows,
                                            fabscheduleaccesslevel=@FabScheduleAccessLevel,
                                            fablinescheduleaccesslevel=@FabLineScheduleAccessLevel,
                                            paintlinescheduleaccesslevel=@PaintLineScheduleAccessLevel,
                                            contractsaccesslevel=@ContractsAccessLevel,
                                            g2updaterversion=@G2UpdaterVersion,
                                            updatelocation_id=@UpdateLocationId,
                                            allocationadmin=@AllocationAdmin,
                                            user_password_last_changed=@UserPasswordLastChanged,
                                            --date_created=@DateCreated,
                                            createdbyuser_id=@CreatedByUserId,
                                            date_modified=GETDATE(),
                                            modifiedbyuser_id=@ModifiedByUserId,
                                            loggedinoncomputer=@LoggedInOnComputer,
                                            yloc=@YLoc,
                                            ylocdsc=@YLocDsc,
                                            locked=@Locked,
                                            fattempt=@FAttempt,
                                            remarks=@Remarks,
                                            releasedt=@ReleaseDt,
                                            releaseby=@ReleaseBy,
                                            inactive=@Inactive,
                                            inactiveremarks=@InactiveRemarks,
                                            inactivereleasedt=@InactiveReleaseDt,
                                            inactivereleaseby=@InactiveReleaseBy,
                                            password_attempts=@PasswordAttempts,
                                            password_updated_flag=@PasswordUpdatedFlag,
                                            unlock_date=@UnlockDate
                                        WHERE user_id=@UserId";

                            using (SqlCommand cmd = new SqlCommand(sql, conn))
                            {
                                // Parameters (mapping request properties)
                                cmd.Parameters.AddWithValue("@UserId", request.UserId);
                                cmd.Parameters.AddWithValue("@UserName", request.UserName);
                                cmd.Parameters.AddWithValue("@UserLoginName", request.UserLoginName);
                                cmd.Parameters.AddWithValue("@UserLoggedOnAt", request.UserLoggedOnAt);
                                cmd.Parameters.AddWithValue("@UserFullName", request.UserFullName);
                                cmd.Parameters.AddWithValue("@UserEmail", request.UserEmail);
                                cmd.Parameters.AddWithValue("@UserInitials", request.UserInitials);
                                cmd.Parameters.AddWithValue("@UserPassword", request.UserPassword);
                                cmd.Parameters.AddWithValue("@UserDepartment", request.UserDepartment);
                                cmd.Parameters.AddWithValue("@UserLoggedIn", request.UserLoggedIn);
                                cmd.Parameters.AddWithValue("@UserInModule", request.UserInModule);
                                cmd.Parameters.AddWithValue("@G2Version", (object)request.G2Version ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@LastLogin", (object)request.LastLogin ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@OS", request.OS);
                                cmd.Parameters.AddWithValue("@CLR", request.CLR);
                                cmd.Parameters.AddWithValue("@UserMenus", request.UserMenus);
                                cmd.Parameters.AddWithValue("@LoginCount", request.LoginCount);
                                cmd.Parameters.AddWithValue("@LibrariesReadonly", request.LibrariesReadOnly);
                                cmd.Parameters.AddWithValue("@GroupCompany", request.GroupCompany);
                                cmd.Parameters.AddWithValue("@Screen1Res", request.Screen1Res);
                                cmd.Parameters.AddWithValue("@Screen2Res", request.Screen2Res);
                                cmd.Parameters.AddWithValue("@Screen3Res", request.Screen3Res);
                                cmd.Parameters.AddWithValue("@Screen4Res", request.Screen4Res);
                                cmd.Parameters.AddWithValue("@PoAuthId", request.POAuthId);
                                cmd.Parameters.AddWithValue("@PoAuthAll", request.POAuthAll);
                                cmd.Parameters.AddWithValue("@OrderAlerts", request.OrderAlerts);
                                cmd.Parameters.AddWithValue("@PoAuthTempUserId", request.POAuthTempUserId);
                                cmd.Parameters.AddWithValue("@MaxOrderValue", request.MaxOrderValue);
                                cmd.Parameters.AddWithValue("@OutOfOffice", request.OutOfOffice);
                                cmd.Parameters.AddWithValue("@DefaultOrderDepartment", request.DefaultOrderDepartment);
                                cmd.Parameters.AddWithValue("@PoRoleId", request.PORoleId);
                                cmd.Parameters.AddWithValue("@Deleted", request.Deleted);
                                cmd.Parameters.AddWithValue("@AliasUserName1", (object)request.AliasUserName1 ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@InvoiceBarcodePrinter", request.InvoiceBarcodePrinter);
                                cmd.Parameters.AddWithValue("@SmtpServer", request.SmtpServer);
                                cmd.Parameters.AddWithValue("@FactoryId", request.FactoryId);
                                cmd.Parameters.AddWithValue("@PieceMonitoringAccessLevel", request.PieceMonitoringAccessLevel);
                                cmd.Parameters.AddWithValue("@ExClassEdit", request.ExClassEdit);
                                cmd.Parameters.AddWithValue("@TimesheetsAccessLevel", request.TimesheetsAccessLevel);
                                cmd.Parameters.AddWithValue("@InitialWindows", request.InitialWindows);
                                cmd.Parameters.AddWithValue("@FabScheduleAccessLevel", request.FabScheduleAccessLevel);
                                cmd.Parameters.AddWithValue("@FabLineScheduleAccessLevel", request.FabLineScheduleAccessLevel);
                                cmd.Parameters.AddWithValue("@PaintLineScheduleAccessLevel", request.PaintLineScheduleAccessLevel);
                                cmd.Parameters.AddWithValue("@ContractsAccessLevel", request.ContractsAccessLevel);
                                cmd.Parameters.AddWithValue("@G2UpdaterVersion", (object)request.G2UpdaterVersion ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@UpdateLocationId", request.UpdateLocationId);
                                cmd.Parameters.AddWithValue("@AllocationAdmin", request.AllocationAdmin);
                                cmd.Parameters.AddWithValue("@UserPasswordLastChanged", (object)request.UserPasswordLastChanged ?? DBNull.Value);
                                //cmd.Parameters.AddWithValue("@DateCreated", (object)request.DateCreated ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@CreatedByUserId", (object)request.CreatedByUserId ?? DBNull.Value);
                                //cmd.Parameters.AddWithValue("@DateModified", (object)request.DateModified ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ModifiedByUserId", (object)request.ModifiedByUserId ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@LoggedInOnComputer", request.LoggedInOnComputer);
                                cmd.Parameters.AddWithValue("@YLoc", (object)request.YLoc ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@YLocDsc", (object)request.YLocDsc ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@Locked", (object)request.Locked ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@FAttempt", (object)request.FAttempt ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@Remarks", (object)request.Remarks ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ReleaseDt", (object)request.ReleaseDt ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@ReleaseBy", (object)request.ReleaseBy ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@Inactive", (object)request.Inactive ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@InactiveRemarks", (object)request.InactiveRemarks ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@InactiveReleaseDt", (object)request.InactiveReleaseDt ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@InactiveReleaseBy", (object)request.InactiveReleaseBy ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@PasswordAttempts", (object)request.PasswordAttempts ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@PasswordUpdatedFlag", (object)request.PasswordUpdatedFlag ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@UnlockDate", (object)request.UnlockDate ?? DBNull.Value);
                                int rowsAffected = cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    scope.Complete();
                }
                return Content(HttpStatusCode.OK, new { Message = Common.Constants.Messages.USER_UPDATED_SUCCESSFULLY });

            }
            catch (Exception ex)
            {
                Logger.Log($"Error in UpdateUser: {ex.Message}");
                Logger.Log($"StackTrace in UpdateUser: { ex.StackTrace.ToString()}");
                return Content(HttpStatusCode.InternalServerError, Common.Constants.Messages.AN_UNEXPECTED_ERROR_OCCURRED);
            }
        }

        [HttpDelete]
        [Route("api/users/delete")]
        public IHttpActionResult DeleteUser([FromBody] DeleteUserRequest request)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var dbKey in request.TargetDatabases)
                    {
                        using (var connection = DbConnectionFactory.GetConnection(dbKey))
                        {
                            connection.Open();
                            using (var command = new SqlCommand("UPDATE Users SET deleted = 1, modifiedbyuser_id = @ModifiedBy, date_modified = GETDATE() WHERE user_id = @UserId", connection))
                            {
                                command.Parameters.AddWithValue("@UserId", request.UserId);
                                command.Parameters.AddWithValue("@ModifiedBy", request.ModifiedByUserId);

                                int rows = command.ExecuteNonQuery();
                            }
                        }
                    }
                    scope.Complete();
                }
                return Content(HttpStatusCode.OK, new { Message = Common.Constants.Messages.USER_DELETED_SUCCESSFULLY });
            }
            catch (Exception ex)
            {

                Logger.Log($"Error in DeleteUser: {ex.Message}");
                Logger.Log($"StackTrace in DeleteUser: { ex.StackTrace.ToString()}");
                return Content(HttpStatusCode.InternalServerError, Common.Constants.Messages.AN_UNEXPECTED_ERROR_OCCURRED);
            }
        }


    }
}
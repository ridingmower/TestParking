using Kztek.Data.AccessEvent.SqlHelper;
using Kztek.Model.CustomModel.Mobile;
using Kztek.Model.Models;
using Kztek.Model.Models.API;
using Kztek.Model.Models.Event;
using Kztek.Web.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing.Imaging;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Kztek.Web.API.Functions
{
    public partial class API_MobileController
    {
        void ProcessCardEventIn(API_EventInOut e, Image imageData, ref tblCardEvent currentEvent, ref string message)
        {
            string _oldcardnumberIn = "";
            DateTime _olddatetimeIn = DateTime.Now;
            bool IsShowingMessageIn = false;
            //Point posL = new Point(200, 300);
            //Point posR = new Point(600, 300);//right

            //bool EnableDeleteCardFailed = false;
            //bool EnableAlarmMessageBoxIn = false;

            //sysConfig
            var currentConfig = _tblSystemConfigService.GetDefault();
            var delayTime = currentConfig.DelayTime;
            var EnableDeleteCardFailed = currentConfig.EnableDeleteCardFailed;
            var EnableAlarmMessageBoxIn = currentConfig.EnableAlarmMessageBoxIn;
            var currentFee = currentConfig.FeeName;

            tblLane currentLane = _API_MobileService.GetLaneById(e.LaneId);
            User currentUser = _API_MobileService.GetUserById(e.UserId);

            //if(!string.IsNullOrWhiteSpace(currentConfig.Para1))
            //{
            //    CardMonthExpireDays = int.Parse(currentConfig.Para1);
            //}

            if (IsShowingMessageIn == true)
                return;

            //STARLAKE
            //StaticPool.InsertNewCardMF(e.CardNumber);
            //if (currentFee.Contains("VINCOM") && IsProcessing == true)
            //{
            //    return;
            //}
            //if (EnableAlarmMessageBoxIn && IsShowingMessageBoxIn == true)
            //    return;

            //string _maincard = StaticPool.GetMainCardFromSubCardnumber(e.CardNumber);
            //if (_maincard != "")
            //    e.CardNumber = _maincard;

            DateTime dtime = DateTime.Now;  //StaticPool.GetDateTimeServer();
            dtime = dtime.AddMilliseconds(-dtime.Millisecond);

            //if (e.Desc != "" && e.Desc.Length == 5)
            //{
            //StaticPool.SaveCardDispenserState(dtime, e);
            //}

            if (e.CardNumber == "")
                return;


            if (e.CardNumber == _oldcardnumberIn && DateDiff(DateInterval.Second, _olddatetimeIn, dtime) < delayTime)
            {
                //if (EnableDeleteCardFailed && _messagetype == CardEventType.CARD_DAY.ToString())
                //{
                //    if (GetPayTypeByCardNumber(e.CardNumber) == 1)
                //    {
                //        var recentEvent = _API_MobileService.GetRecentEventInByCardnumber(e.CardNumber);

                //        if (recentEvent != null)
                //        {
                //            recentEvent.IsDelete = true;
                //            recentEvent.EventCode = "2";
                //            //TODO : Update event to db

                //            _oldcardnumberIn = "";
                //            _olddatetimeIn = dtime;
                //            lbWarningIn = "XÓA THẺ";
                //        }
                //    }
                //}
                //return;
            }
            else
            {
                _olddatetimeIn = dtime;
                message = ProcessEventIn(e, dtime, currentLane, currentUser, imageData, ref currentEvent, ref _oldcardnumberIn);
            }
        }

        string ProcessEventIn(API_EventInOut e, DateTime dtime, tblLane currentLane, User currentUser, Image imageData, ref tblCardEvent currentEvent, ref string _oldcardnumberIn)
        {
            API_CardInfo cardInfo = null;
            var msg = "";

            Image picCustomer;

            //sysConfig
            var currentConfig = _tblSystemConfigService.GetDefault();
            var delayTime = currentConfig.DelayTime;
            var EnableDeleteCardFailed = currentConfig.EnableDeleteCardFailed;
            var EnableAlarmMessageBoxIn = currentConfig.EnableAlarmMessageBoxIn;
            var currentFee = currentConfig.FeeName;

            bool IsShowingMessageIn = false;
            bool EventFlagIn = false;
             
            string plate = "";
            Bitmap bmpCut = (Bitmap)imageData;
            Bitmap bmpPic = (Bitmap)imageData;
            string picdir = CreatFileNameIn(dtime) + "PLATEIN.JPG";
            string picdiroverview = picdir.Replace("PLATEIN.JPG", "OVERVIEWIN.JPG");
            string picdirplatecut = picdir.Replace("PLATEIN.JPG", "PLATECUTIN.JPG");

            //determind bike or car
            bool isCar = false;
            bool cardexist = false;

            cardInfo = _API_MobileService.GetCardInfoByCardNumber(e.CardNumber);

            if (cardInfo == null)
            {
                msg = "THẺ " + e.CardNumber + " KHÔNG TỒN TẠI TRONG HỆ THỐNG";
                SaveEventAlarm(dtime, e.CardNumber, plate, picdir, AlarmType.INVALID_CARD, "");
                //Lưu ảnh sự kiện
                SavePicture(bmpPic, picdir);
                SavePicture(bmpPic, picdiroverview);

                return msg;
            }
            _oldcardnumberIn = e.CardNumber;

            if (cardInfo != null)
            {
                if (cardInfo.VehicleGroupName != null)
                    isCar = true;
                else
                    isCar = false;
            }

            if (isCar)
            {
                plate = ""; //GetPlate(currentFee, bmpPic, ref bmpCut);
                if (currentLane.AccessForEachSide == true && cardexist == true)
                {
                    //access denied
                    msg = "SAI QUYỀN TRUY CẬP";
                    if (bmpPic != null)
                        bmpPic.Save(picdir, ImageFormat.Jpeg);
                    SaveEventAlarm(dtime, e.CardNumber, plate, picdir, AlarmType.INACCESSABLE, "");
                    return msg;
                }
            }
            else
            {
                plate = ""; //GetPlate(currentFee, bmpPic, ref bmpCut);
                if (currentLane.AccessForEachSide == true && cardexist == true)
                {
                    //access denied
                    msg = "SAI QUYỀN TRUY CẬP";
                    //lsbMessageIn.Items.Add(_cardnumber);
                    if (bmpPic != null)
                        bmpPic.Save(picdir, ImageFormat.Jpeg);
                    SaveEventAlarm(dtime, e.CardNumber, plate, picdir, AlarmType.INACCESSABLE, "");
                    return msg;
                }

            }

            //if (LoopSuportFarReader && e.Desc != "")
            //    plate = e.Desc;

            //picPlateIn = bmpPic;
            //picOverViewIn = camOverViewIn.GetCurrentImage();
            //picPlateCutIn = bmpCut;

            //if (picPlateIn != null)
            //{
            //    Image img = new Bitmap(picPlateIn);
            //    SavePicture(img, picdir);
            //}

            //if (picOverViewIn != null)
            //{
            //    Image img = new Bitmap(picOverViewIn);
            //    SavePicture(img, picdiroverview);
            //}

            //if (picPlateCutIn != null)
            //{
            //    Image img = new Bitmap(picPlateCutIn);
            //    SavePicture(img, picdirplatecut);
            //}

            SavePicture(bmpPic, picdir);
            SavePicture(bmpPic, picdiroverview);
            SavePicture(bmpCut, picdirplatecut);

            //if (currentFee == "VINCOM")
            //{
            //    string _des = "";
            //    if (CheckBlackList_VINCOM(plate, ref _des) == true)
            //    {
            //        IsProcessing = true;
            //        if (MessageBox.Show("BIỂN SỐ ĐEN, BẠN CÓ ĐỒNG Ý CHO XE VÀO KHÔNG?" + Environment.NewLine + _des.ToUpper(), "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //        {
            //            IsProcessing = false;
            //            return "";
            //        }
            //        IsProcessing = false;
            //    }
            //}

            var _msg = new MessageResult();

            ProcessEventIn_PE(plate, picdir, dtime, currentFee, cardInfo, currentLane, currentUser, ref currentEvent, ref _msg);

            if (_msg.MessageType == AlarmType.PLATE_NOT_SAME.ToString())
            {
                _oldcardnumberIn = "";
            }

            msg = _msg.MessageContent;

            if (_msg.MessageType == CardEventType.CARD_DAY.ToString() || _msg.MessageType == CardEventType.CARD_MONTH.ToString())
            {
                if (_msg.CardEventInID == "")
                {
                    msg = "LƯU DỮ LIỆU KHÔNG THÀNH CÔNG";
                    return msg;
                }

                if (EnableAlarmMessageBoxIn == true && _msg.IsOpenBarrie == false && _msg.MessageType == CardEventType.CARD_MONTH.ToString())
                {
                    IsShowingMessageIn = true;

                    if (currentFee == "TAT")
                    {
                        //frmEditPlate2 _frmeditplate2 = new frmEditPlate2();
                        //_frmeditplate2.RegPlate = msg.RegistedPlate.Replace("-", "").Replace(".", "").Replace(" ", "");
                        //_frmeditplate2.Plate = msg.PlateIn;

                        //if (_frmeditplate2.ShowDialog() == DialogResult.OK)
                        //{
                        //    StaticPool.mdbevent.ExecuteCommand("update tblCardEvent set PlateIn=N'" + _frmeditplate2.Plate + "'  where Id='" + msg.CardEventInID + "'");
                        //    //txtPlateIn.Text = _frmeditplate2.Plate;
                        //    lbWarningIn = "MỜI VÀO";
                        //    SaveEventAlarm(dtime, _cardnumber, txtPlateIn.Text, picdir, AlarmType.PLATE_NOT_SAME, "Khác biển số");
                        //}
                        //else
                        //{
                        //    _oldcardnumberIn = "";
                        //    StaticPool.mdbevent.ExecuteCommand("update tblCardEvent set EventCode='2', IsDelete=1 where Id='" + msg.CardEventInID + "'");

                        //    return "";
                        //}
                    }
                    else
                    {
                        //if (MessageBox.Show("Biển số không khớp, bạn có đồng ý cho xe vào không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        //{
                        //    ExcuteSQLEvent.Execute("update tblCardEvent set EventCode='2', IsDelete=1 where Id='" + msg.CardEventInID + "'");
                        //    _oldcardnumberIn = "";

                        //    return "";
                        //}

                        //ExcuteSQLEvent.Execute("update tblCardEvent set EventCode='2', IsDelete=1 where Id='" + msg.CardEventInID + "'");
                        //_oldcardnumberIn = "";
                    }
                    IsShowingMessageIn = false;
                    _msg.IsOpenBarrie = true;
                }

                if (_msg.IsOpenBarrie)
                {
                    if (currentFee == "HANDICO" && isCar == false)
                    {

                    }
                    else
                    {
                        //OpenDoor(e, "L");
                    }
                }
                else
                    EventFlagIn = true;

                //sendtoled
                //SendToLed(dspLeft, msg.DatetimeIn.ToString("yyyy/MM/dd HH:mm"), "", msg.PlateIn, "0", msg.CardType, msg.CardState);

                if (_msg.MessageType == CardEventType.CARD_MONTH.ToString())
                {
                    //display customer's image
                    if (_msg.Avatar != "" && _msg.Avatar.Split('/').Length == 3)
                    {
                        string _piccustomer = "";// StaticPool.PicPathCustomer + @"\" + msg.Avatar.Split('/')[1] + @"\" + msg.Avatar.Split('/')[2];
                        if (File.Exists(_piccustomer))
                        {
                            picCustomer = Image.FromFile(_piccustomer);
                        }
                    }
                }
            }
            else
            {
                //picPlateIn = null;
                //picOverViewIn = null;
                if (_msg.CardState != "")
                {
                    //sendtoled
                    //SendToLed(dspLeft, msg.DatetimeIn.ToString("yyyy/MM/dd HH:mm"), "", msg.PlateIn, "0", msg.CardType, msg.CardState);
                }
            }

            //OpenDoorTVG(3);

            //return _msg.MessageType;
            return msg;
        }

        void ProcessEventIn_PE(string plate, string picdir, DateTime dtime, string currentFee, API_CardInfo cardInfo, tblLane currentLane, User currentUser, ref tblCardEvent currentEvent, ref MessageResult msg)
        {
            int CardMonthExpireDays = 15;

            msg.Cardnumber = cardInfo.CardNumber;

            bool IsHaveMoneyExpiredDate = false;
            //black list
            if (CheckBlackList_PE(plate))
            {
                msg.IsBlackList = true;
                SaveEventAlarm(dtime, cardInfo.CardNumber, plate, picdir, AlarmType.BLACKLIST, "");
            }

            if (cardInfo.CardGroupName != null)
            {
                msg.CardType = cardInfo.CardType;
                IsHaveMoneyExpiredDate = cardInfo.IsHaveMoneyExpiredDate;
                if (cardInfo.CardGroupInactive == true)
                {
                    msg.MessageType = CardEventType.CARD_LOCK.ToString();
                    msg.MessageContent = "NHÓM THẺ BỊ KHÓA";
                    msg.MessageColor = Color.Red;
                    SaveEventAlarm(dtime, cardInfo.CardNumber, plate, picdir, AlarmType.CARDGROUP_LOCK, "");

                    return;
                }
            }

            msg.Cardgroup = cardInfo.CardGroupName;
            msg.CardNo = cardInfo.CardNo;
            if (cardInfo.IsLock)
            {
                msg.MessageType = CardEventType.CARD_LOCK.ToString();
                msg.MessageContent = "THẺ BỊ KHÓA";
                if (!string.IsNullOrWhiteSpace(cardInfo.CardDescription))
                {
                    msg.MessageContent = msg.MessageContent + "-" + cardInfo.CardDescription;
                }
                msg.MessageColor = Color.Red;
                SaveEventAlarm(dtime, cardInfo.CardNumber, plate, picdir, AlarmType.CARD_LOCK, "");

                return;
            }

            //check activedate
            if (cardInfo.DateActive != null && (DateTime)cardInfo.DateActive > dtime)
            {
                msg.MessageType = CardEventType.CARD_EXPIRE.ToString();
                msg.MessageContent = "THẺ CHƯA KÍCH HOẠT";
                msg.MessageColor = Color.Red;
                SaveEventAlarm(dtime, cardInfo.CardNumber, plate, picdir, AlarmType.CARD_EXPIRED, "");
                return;
            }

            //check accesslevel
            if (CheckAccessLevel(cardInfo.CardGroupID, currentLane.LaneID.ToString()) == false)
            {
                msg.MessageType = CardEventType.INACCESSABLE.ToString();
                msg.MessageContent = "SAI QUYỀN TRUY CẬP";
                msg.MessageColor = Color.Red;
                SaveEventAlarm(dtime, cardInfo.CardNumber, plate, picdir, AlarmType.INACCESSABLE, "");
                return;
            }

            //check xe trong bai
            var _recentEvent = _API_MobileService.GetRecentEventInByCardnumber(cardInfo.CardNumber);
            if (_recentEvent != null)
            {
                msg.MessageType = CardEventType.VEHICLE_GOT_IN.ToString();
                msg.MessageContent = "XE ĐÃ VÀO BÃI";
                msg.MessageColor = Color.Red;
                SaveEventAlarm(dtime, cardInfo.CardNumber, plate, picdir, AlarmType.VEHICLE_GOT_IN, "");
                return;
            }

            //check antipassback
            //DataTable dtantipassback = StaticPool.mdbevent.FillData("select * from tblCardEvent where EventCode='1' and Cardnumber='" + cardnumber + "'");
            //if (dtantipassback != null && dtantipassback.Rows.Count > 0)
            //{
            //    if (currentFee == "COMA6_BIKE")
            //    {
            //        if (COMA6_GotIn(cardnumber, plate, dtantipassback) == true)
            //        {
            //            msg.MessageType = CardEventType.VEHICLE_GOT_IN.ToString();
            //            msg.MessageContent = "XE ĐÃ VÀO BÃI";
            //            msg.MessageColor = Color.Red;
            //            SaveEventAlarm(dtime, cardnumber, plate, picdir, AlarmType.VEHICLE_GOT_IN, "");
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        msg.MessageType = CardEventType.VEHICLE_GOT_IN.ToString();
            //        msg.MessageContent = "XE ĐÃ VÀO BÃI";
            //        msg.MessageColor = Color.Red;
            //        SaveEventAlarm(dtime, cardnumber, plate, picdir, AlarmType.VEHICLE_GOT_IN, "");
            //        return;
            //    }
            //}
            //else if (dtantipassback == null || (dtantipassback != null && dtantipassback.Rows.Count == 0))// check again
            //{
            //    dtantipassback = StaticPool.mdbevent.FillData("select * from tblCardEvent where EventCode='1' and Cardnumber='" + cardnumber + "'");
            //    if (dtantipassback != null && dtantipassback.Rows.Count > 0)
            //    {
            //        if (currentFee == "COMA6_BIKE")
            //        {
            //            if (COMA6_GotIn(cardnumber, plate, dtantipassback) == true)
            //            {
            //                msg.MessageType = CardEventType.VEHICLE_GOT_IN.ToString();
            //                msg.MessageContent = "XE ĐÃ VÀO BÃI";
            //                msg.MessageColor = Color.Red;
            //                SaveEventAlarm(dtime, cardnumber, plate, picdir, AlarmType.VEHICLE_GOT_IN, "");
            //                return;
            //            }
            //        }
            //        else
            //        {
            //            msg.MessageType = CardEventType.VEHICLE_GOT_IN.ToString();
            //            msg.MessageContent = "XE ĐÃ VÀO BÃI";
            //            msg.MessageColor = Color.Red;
            //            SaveEventAlarm(dtime, cardnumber, plate, picdir, AlarmType.VEHICLE_GOT_IN, "");
            //            return;
            //        }
            //    }
            //}

            //if (dtantipassback == null)
            //    return;

            //process for HHT

            //if (currentFee == "HHT" && (cardgroupID == "78957e8d-aada-44ce-9047-096c4c86ae20" || cardgroupID == "d3d45321-a8b9-4304-bcd8-a6d4852efe0f"))
            //{
            //    DataTable dteventout = StaticPool.mdbevent.FillData("select top 1 CardNumber, DateTimeOut from tblCardEvent where IsDelete=0 and EventCode='2' and CardNumber='" + cardnumber + "' order by DateTimeOut desc");
            //    if (dteventout != null && dteventout.Rows.Count == 1)
            //    {
            //        long _mdiffout = DateDiff(DateInterval.Second, DateTime.Parse(dteventout.Rows[0]["DateTimeOut"].ToString()), dtime);
            //        if (_mdiffout < 30)
            //            return;
            //    }
            //}

            ////process for lang ha
            //if (currentFee == "LANGHA" ||
            //    currentFee == "CUADONG"
            //    )
            //{
            //    DataTable dteventout = StaticPool.mdbevent.FillData("select top 1 CardNumber, DateTimeOut from tblCardEvent where IsDelete=0 and EventCode='2' and CardNumber='" + cardnumber + "' order by DateTimeOut desc");
            //    if (dteventout != null && dteventout.Rows.Count == 1)
            //    {
            //        long _mdiffout = DateDiff(DateInterval.Second, DateTime.Parse(dteventout.Rows[0]["DateTimeOut"].ToString()), dtime);
            //        if (_mdiffout < 50)
            //            return;
            //    }
            //}

            msg.Cardgroup = cardInfo.CardGroupName;
            msg.Vehicle = cardInfo.VehicleGroupName;
            //if month card or free card
            msg.PayType = cardInfo.CardGroupName != null ? cardInfo.CardType : -1;
            if (msg.PayType == 0 || msg.PayType == 2)//month or vip
            {
                if (cardInfo.CustomerName != null)
                {
                    msg.CustomerCode = cardInfo.CustomerCode;
                    msg.CustomerName = cardInfo.CustomerName;
                    msg.Address = cardInfo.Address;
                    msg.IDNumber = cardInfo.IDNumber;
                    msg.Mobile = cardInfo.Mobile;
                }

                if (cardInfo.CustomerGroupName != null)
                {
                    msg.CustomerGroup = cardInfo.CustomerGroupName;
                }

                //check registed plate
                string _plate1 = cardInfo.Plate1;
                string _plate2 = cardInfo.Plate2;
                string _plate3 = cardInfo.Plate3;
                msg.RegistedPlate = _plate1;
                msg.VehicleName = cardInfo.VehicleName1;
                if (!string.IsNullOrWhiteSpace(_plate2))
                    msg.RegistedPlate = msg.RegistedPlate + ";" + _plate2;
                if (!string.IsNullOrWhiteSpace(_plate3))
                    msg.RegistedPlate = msg.RegistedPlate + ";" + _plate3;

                if (msg.PayType == 0)
                {
                    DateTime dtimeexpire = DateTime.Now.AddDays(-1);//DateTime.Parse(drvcard["ExpireDate"].ToString());
                    if (cardInfo.ExpireDate != null)
                        dtimeexpire = (DateTime)cardInfo.ExpireDate;
                    if (dtimeexpire < dtime && IsHaveMoneyExpiredDate == false)
                    {
                        //check expire 
                        msg.MessageType = CardEventType.CARD_EXPIRE.ToString();
                        msg.MessageContent = "HẾT HẠN SD" + "-" + dtimeexpire.ToString("dd/MM/yyyy");
                        msg.MessageColor = Color.Red;
                        SaveEventAlarm(dtime, cardInfo.CardNumber, plate, picdir, AlarmType.CARD_EXPIRED, "");
                        //OutputByAudio("TheHetHan.wav");
                        msg.CardState = "expired";
                        return;
                    }
                    if (currentFee == "LVT"
                        || currentFee == "AQUA"
                        )
                    {
                        msg.MessageContentEx = "THỜI HẠN THẺ: " + dtimeexpire.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        long _ddiff = DateDiff(DateInterval.Day, dtime, dtimeexpire);
                        if (_ddiff <= CardMonthExpireDays)
                        {
                            if (_ddiff == 0 && dtime > dtimeexpire.AddDays(1))
                                msg.MessageContentEx = "HẾT HẠN SD" + "-" + dtimeexpire.ToString("dd/MM/yyyy");
                            else if (_ddiff > 0 || (_ddiff == 0 && dtime > dtimeexpire))
                                msg.MessageContentEx = "THSD THẺ CÒN " + _ddiff.ToString() + " NGÀY";
                            msg.CardState = "underdeadline";
                        }
                    }
                }


                bool isAlarm = true;

                //if (ComparePlate(plate, msg.RegistedPlate, "IN", cardgroupID) == true)
                //{
                //    isAlarm = false;
                //}


                msg.VehicleName = cardInfo.VehicleName1;
                if (!string.IsNullOrWhiteSpace(cardInfo.VehicleName2))
                    msg.VehicleName = msg.VehicleName + ";" + cardInfo.VehicleName2;
                if (!string.IsNullOrWhiteSpace(cardInfo.VehicleName3))
                    msg.VehicleName = msg.VehicleName + ";" + cardInfo.VehicleName3;



                msg.MessageType = CardEventType.CARD_MONTH.ToString();

                msg.DatetimeIn = dtime;
                msg.PlateIn = plate;

                if (isAlarm == false)
                {
                    msg.MessageContent = "MỜI VÀO";
                    msg.MessageColor = Color.Lime;
                    msg.IsOpenBarrie = true;
                }
                else
                {
                    if (currentLane.CheckPlateLevelIn == 3)
                    {
                        msg.MessageContent = "MỜI VÀO";
                        msg.MessageColor = Color.Lime;
                        msg.IsOpenBarrie = false;
                    }
                    else
                    {
                        msg.MessageContent = "CẢNH BÁO BIỂN SỐ";
                        msg.MessageColor = Color.Red;
                        msg.IsOpenBarrie = false;
                    }

                }


                var bl = CreateCardEventIn(dtime, cardInfo.CardNumber, plate, picdir, cardInfo.CustomerName, msg.RegistedPlate, cardInfo.CardGroupID, cardInfo.VehicleGroupID, cardInfo.CustomerGroupID, msg.IsBlackList, msg.CardNo, currentUser.Id.ToString(), currentLane.LaneID.ToString(), ref currentEvent);

                msg.CardEventInID = bl ? currentEvent.Id.ToString() : "";
                //if (msg.CardEventInID == "")
                //{
                //    SystemUI.SaveLogFile("SaveCardEventIn failed-1");
                //}

                msg.Avatar = cardInfo.Avatar;

                if (currentFee == "UDICCOMPLEX" && msg.MessageContentEx.Contains("HẾT HẠN SD"))
                {
                    //OutputByAudio("TheHetHan.wav");
                }

                if (currentFee == "BVTMHMT" && cardInfo.VehicleGroupID != "84a002e0-34f3-46da-9b69-1fa76fcf4c91")
                    msg.IsOpenBarrie = false;
            }
            else //day
            {

                msg.MessageType = CardEventType.CARD_DAY.ToString();
                msg.MessageContent = "MỜI VÀO";

                if (plate != "")
                {
                    string _plate = plate.Replace(" ", "").Replace("-", "").Replace(".", "");

                    var _dsTemp = Data.SqlHelper.ExcuteSQL.GetDataSet("select * from tblCard where IsDelete=0 and IsLock=0 and" +
                      " (replace(replace(replace(Plate1,'.',''),'-',''),' ','')='" + _plate +
                      "' or replace(replace(replace(Plate2,'.',''),'-',''),' ','')='" + _plate +
                      "' or replace(replace(replace(Plate3,'.',''),'-',''),' ','')='" + _plate +
                      "')");

                    if (_dsTemp.Tables.Count > 0 && _dsTemp.Tables[0].Rows.Count > 0)
                    {
                        msg.RegistedPlate = "BS THUÊ BAO";
                    }
                }

                if (plate == "")
                    msg.MessageColor = Color.Yellow;
                else
                    msg.MessageColor = Color.Lime;

                msg.DatetimeIn = dtime;
                msg.PlateIn = plate;

                msg.IsOpenBarrie = true;

                var bl = CreateCardEventIn(dtime, cardInfo.CardNumber, plate, picdir, "", "", cardInfo.CardGroupID, cardInfo.VehicleGroupID, "", msg.IsBlackList, msg.CardNo, currentUser.Id.ToString(), currentLane.LaneID.ToString(), ref currentEvent);

                msg.CardEventInID = bl ? currentEvent.Id.ToString() : "";
                //if (msg.CardEventInID == "")
                //{
                //    SystemUI.SaveLogFile("SaveCardEventIn failed-2");
                //}
                if (currentFee == "BVTMHMT" && cardInfo.VehicleGroupID != "84a002e0-34f3-46da-9b69-1fa76fcf4c91")
                    msg.IsOpenBarrie = false;
            }

            //check số lượt ra vào bãi xe trong tháng FPT_DUYTAN-23 / 11 / 19
            //if (currentFee == "FPT_DUYTAN")
            //{
            //    if (paytype != 0 && paytype != 2 && plate != "")
            //    {
            //        int dtMonth = Convert.ToInt32(dtime.Month);
            //        int dtYear = Convert.ToInt32(dtime.Year);
            //        string startMonth = dtYear.ToString() + "-" + dtMonth.ToString() + "-01 00:00:00.000";
            //        string endMonth = dtMonth + 1 <= 12 ? dtYear.ToString() + "-" + (dtMonth + 1).ToString() + "-01 00:00:00.000" : (dtYear + 1).ToString() + "-01-01 00:00:00.000";

            //        //var numberTurn = OverTurn("19B-003.17", startMonth, endMonth, "d3d45321-a8b9-4304-bcd8-a6d4852efe0f"); //test
            //        var numberTurn = OverTurn(plate, startMonth, endMonth, cardgroupID);
            //        if (numberTurn > 10)
            //        {
            //            msg.MessageType = CardEventType.CARD_DAY.ToString();
            //            msg.MessageContentEx = "Xe vào quá 10 lượt trong tháng";
            //            msg.MessageColor = Color.Red;
            //            return;
            //        }
            //    }
        }

        void ProcessCardEventOut(API_EventInOut e, Image imageData, ref tblCardEvent currentEvent, ref string message)
        {
            string _oldcardnumberOut = "";
            DateTime _olddatetimeOut = DateTime.Now;
            bool IsShowingMessageOut = false;
            //Point posL = new Point(200, 300);
            //Point posR = new Point(600, 300);//right

            //bool EnableDeleteCardFailed = false;
            //bool EnableAlarmMessageBoxIn = false;

            //sysConfig
            var currentConfig = _tblSystemConfigService.GetDefault();
            var delayTime = currentConfig.DelayTime;
            var EnableDeleteCardFailed = currentConfig.EnableDeleteCardFailed;
            var EnableAlarmMessageBoxIn = currentConfig.EnableAlarmMessageBoxIn;
            var currentFee = currentConfig.FeeName;

            tblLane currentLane = _API_MobileService.GetLaneById(e.LaneId);
            User currentUser = _API_MobileService.GetUserById(e.UserId);

            //if(!string.IsNullOrWhiteSpace(currentConfig.Para1))
            //{
            //    CardMonthExpireDays = int.Parse(currentConfig.Para1);
            //}

            if (IsShowingMessageOut == true)
                return;
            //StaticPool.InsertNewCardMF(e.CardNumber);

            //string _maincard = StaticPool.GetMainCardFromSubCardnumber(e.CardNumber);
            //if (_maincard != "")
            //    e.CardNumber = _maincard;

            //if (currentFee.Contains("VINCOM") && IsProcessing == true)
            //{
            //    return;
            //}
            //if (StaticPool.EnableAlarmMessageBox == true && ProcessEvent.IsShowingMessageBoxOut == true)
            //    return;
            DateTime dtime = DateTime.Now;
            dtime = dtime.AddMilliseconds(-dtime.Millisecond);
            //if (StaticPool.GetReaderOfScreen(e) == "L")
            {
                if (e.CardNumber == _oldcardnumberOut && DateDiff(DateInterval.Second, _olddatetimeOut, dtime) < delayTime)
                {
                    //return;
                }
                else
                {
                    message = ProcessEventOut(e, dtime, currentLane, currentUser, imageData, ref currentEvent, ref _oldcardnumberOut);
                }
            }
        }

        string ProcessEventOut(API_EventInOut e, DateTime dtime, tblLane currentLane, User currentUser, Image imageData, ref tblCardEvent currentEvent, ref string _oldcardnumberOut)
        {
            var msg = "";
            API_CardInfo cardInfo = null;

            Image picCustomer;

            //sysConfig
            var currentConfig = _tblSystemConfigService.GetDefault();
            var delayTime = currentConfig.DelayTime;
            var EnableDeleteCardFailed = currentConfig.EnableDeleteCardFailed;
            var EnableAlarmMessageBox = currentConfig.EnableAlarmMessageBox;
            var EnableAlarmMessageBoxIn = currentConfig.EnableAlarmMessageBoxIn;
            var currentFee = currentConfig.FeeName;

            bool IsShowingMessageIn = false;
            bool EventFlagIn = false;

            // ClearOut();
            //EventFlag = true;
            string plate = "";
            Bitmap bmpCut = (Bitmap)imageData;
            Bitmap bmpPic = (Bitmap)imageData;
            string picdir = CreatFileNameOut(dtime) + "PLATEOUT.JPG";
            string picdiroverview = picdir.Replace("PLATEOUT.JPG", "OVERVIEWOUT.JPG");
            string picdirplatecut = picdir.Replace("PLATEOUT.JPG", "PLATECUTOUT.JPG");

            bool isCar = false;
            bool cardexist = false;
            bool IsValidCardEvent = false;

            cardInfo = _API_MobileService.GetCardInfoByCardNumber(e.CardNumber);

            if (cardInfo == null)
            {
                msg = "THẺ " + e.CardNumber + " KHÔNG TỒN TẠI TRONG HỆ THỐNG";
                SaveEventAlarm(dtime, e.CardNumber, plate, picdir, AlarmType.INVALID_CARD, "");
                //Lưu ảnh sự kiện
                SavePicture(bmpPic, picdir);
                SavePicture(bmpPic, picdiroverview);

                return msg;
            }
            _oldcardnumberOut = e.CardNumber;

            if (cardInfo != null)
            {
                if (cardInfo.VehicleGroupName != null)
                    isCar = true;
                else
                    isCar = false;
            }

            if (isCar)
            {
                plate = "";//GetPlate(currentFee, bmpPic, ref bmpCut);
                if (currentLane.AccessForEachSide == true && cardexist == true)
                {
                    //access denied
                    msg = "SAI QUYỀN TRUY CẬP";
                    if (bmpPic != null)
                        bmpPic.Save(picdir, ImageFormat.Jpeg);
                    SaveEventAlarm(dtime, e.CardNumber, plate, picdir, AlarmType.INACCESSABLE, "");
                    return msg;
                }
            }
            else
            {
                plate = ""; //GetPlate(currentFee, bmpPic, ref bmpCut);
                if (currentLane.AccessForEachSide == true && cardexist == true)
                {
                    //access denied
                    if (bmpPic != null)
                        bmpPic.Save(picdir, ImageFormat.Jpeg);
                    SaveEventAlarm(dtime, e.CardNumber, plate, picdir, AlarmType.INACCESSABLE, "");
                    return msg;
                }
            }

            //if (StaticPool.LoopSuportFarReader == true && e.Desc != "")
            //    plate = e.Desc;

            //picPlateOut.Image = bmpPic;
            //picOverViewOut.Image = camOverViewOut.GetCurrentImage();
            //picPlateCutOut.Image = bmpCut;

            //if (picPlateOut.Image != null)
            //{
            //    //picPlateOut.Image.Save(picdir, ImageFormat.Jpeg);
            //    Image img = new Bitmap(picPlateOut.Image);
            //    SavePicture(img, picdir);
            //}
            //if (picOverViewOut.Image != null)
            //{
            //    // picOverViewOut.Image.Save(picdiroverview, ImageFormat.Jpeg);
            //    Image img = new Bitmap(picOverViewOut.Image);
            //    SavePicture(img, picdiroverview);
            //}

            SavePicture(bmpPic, picdir);
            SavePicture(bmpPic, picdiroverview);
            SavePicture(bmpPic, picdirplatecut);

            var _msg = new MessageResult();

            ProcessEventOut_PE(plate, picdir, dtime, currentFee, cardInfo, currentLane, currentUser, ref currentEvent, ref _msg);
            msg = _msg.MessageContent;
            if (_msg.MessageType == CardEventType.CARD_DAY.ToString() || _msg.MessageType == CardEventType.CARD_MONTH.ToString())
            {
                IsValidCardEvent = true;
            }

            if (_msg.MessageType == AlarmType.PLATE_NOT_SAME.ToString())
            {
                _oldcardnumberOut = "";
            }

            if (File.Exists(_msg.PicDirIn))
            { }
            string _picdiroverviewIn = _msg.PicDirIn.Replace("PLATEIN.JPG", "OVERVIEWIN.JPG");
            string _picdircutIn = _msg.PicDirIn.Replace("PLATEIN.JPG", "PLATECUTIN.JPG");

            //var lbMoney = string.Format("{0:0,0}", _msg.Money);


            if (_msg.MessageType == CardEventType.CARD_DAY.ToString() || _msg.MessageType == CardEventType.CARD_MONTH.ToString())
            {
                //IsValidCardEvent = true;
                if (_msg.CardEventOutID == "")
                {
                    msg = "LƯU DỮ LIỆU KHÔNG THÀNH CÔNG";
                    return msg;
                }

                if ((EnableAlarmMessageBox == true && (_msg.MessageType == CardEventType.CARD_MONTH.ToString() || _msg.MessageType == CardEventType.CARD_DAY.ToString()) && _msg.IsAlarm == true) ||
                    (currentFee == "GERMANHOUSE" && (_msg.MessageType == CardEventType.CARD_MONTH.ToString() || _msg.MessageType == CardEventType.CARD_DAY.ToString())) ||
                     (currentFee == "MASTERI" && (_msg.MessageType == CardEventType.CARD_MONTH.ToString() || _msg.MessageType == CardEventType.CARD_DAY.ToString())) ||
                    (currentFee == "GOLDVIEW" && _msg.MessageType == CardEventType.CARD_DAY.ToString())
                    )
                {
                    string _mes = "Biển số không khớp, bạn có đồng ý cho xe ra không?";
                    if (currentFee == "GERMANHOUSE" || currentFee == "MASTERI" || currentFee == "GOLDVIEW")
                    {
                        _mes = "Bạn có đồng ý cho xe ra không?";
                    }

                    //IsShowingMessageOut = true;

                    if (currentFee == "TAT" && _msg.MessageType == CardEventType.CARD_MONTH.ToString())
                    {
                        //frmEditPlate2 _frmeditplate2 = new frmEditPlate2();
                        //_frmeditplate2.RegPlate = msg.RegistedPlate.Replace("-", "").Replace(".", "").Replace(" ", "");
                        //_frmeditplate2.Plate = msg.PlateOut.Replace("-", "").Replace(".", "").Replace(" ", "");

                        //// _frmeditplate2.Location = new Point(posR.X, posR.Y);

                        //if (_frmeditplate2.ShowDialog() == DialogResult.OK)
                        //{
                        //    StaticPool.mdbevent.ExecuteCommand("update tblCardEvent set PlateOut=N'" + _frmeditplate2.Plate + "'  where Id='" + msg.CardEventOutID + "'");
                        //    txtPlateOut.Text = _frmeditplate2.Plate;
                        //    lbWarningOut.Text = "HẸN GẶP LẠI";
                        //    lbWarningOut.BackColor = Color.Lime;
                        //}
                        //else
                        //{
                        //    StaticPool.mdbevent.ExecuteCommand("update tblCardEvent set EventCode='1' where Id='" + msg.CardEventOutID + "'");
                        //    ClearOut();
                        //    _oldcardnumberOut = "";
                        //    return;
                        //}
                    }
                    else
                    {
                        //if (MessageBox.Show(_mes, "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        //{
                        //    StaticPool.mdbevent.ExecuteCommand("update tblCardEvent set EventCode='1' where Id='" + msg.CardEventOutID + "'");
                        //    ClearOut();
                        //    _oldcardnumberOut = "";
                        //    return;
                        //}
                    }
                    _msg.IsOpenBarrie = true;
                    //IsShowingMessageOut = false;
                }

                //print info
                //if (currentFee == "NOIBAI")
                {
                    //btPrint.Tag =
                    //    msg.DatetimeIn.ToString("yyyy/MM/dd HH:mm:ss") +
                    //    ";" + msg.DatetimeOut.ToString("yyyy/MM/dd HH:mm:ss") +
                    //    ";" + msg.Cardgroup +
                    //    ";" + msg.PayType +
                    //    ";" + msg.Cardnumber +
                    //    ";" + msg.PlateIn +
                    //    ";" + StaticPool.GetPeriodTime(msg.DatetimeIn, msg.DatetimeOut) +
                    //    ";" + string.Format("{0:0,0}", msg.Money) +
                    //    ";" + "0";
                    //btPrint.Tag = msg;
                    //((MessageResult)btPrint.Tag).HavePayMoney = " /Tiền phải trả: " + lbMoney.Text;
                }
                // else
                {
                    //btPrint.Tag =
                    //   "Mã thẻ: " + msg.Cardnumber +
                    //   ";CardNo: " + msg.CardNo +
                    //   ";Biển số: " + msg.PlateIn +
                    //   ";Thời gian vào: " + msg.DatetimeIn.ToString("dd/MM/yyyy HH:mm:ss") +
                    //   ";Thời gian ra: " + msg.DatetimeOut.ToString("dd/MM/yyyy HH:mm:ss") +
                    //   ";Thời gian đỗ: " + StaticPool.GetPeriodTime(msg.DatetimeIn, msg.DatetimeOut) +
                    //    (msg.AvailableTimeDay == 0 ? "" : (";TG đã SD/Tối đa ngày: " + StaticPool.GetTimeDetailFromMinutes(msg.UsedTimeDay) + "/" + StaticPool.GetTimeDetailFromMinutes(msg.AvailableTimeDay))) +
                    //    (msg.AvailableTimeMonth == 0 ? "" : (";TG đã SD/Tối đa tháng: " + StaticPool.GetTimeDetailFromMinutes(msg.UsedTimeMonth) + "/" + StaticPool.GetTimeDetailFromMinutes(msg.AvailableTimeMonth))) +
                    //   ";Số tiền: " + string.Format("{0:0,0}", msg.Money);
                }

                //if (StaticPool.IsPrint && msg.Money > 0)
                //{
                //    btPrint_Click(null, null);
                //}

                //if (msg.Money > 0)
                //{
                //    btFree.Tag = msg;
                //}

                //if (msg.IsOpenBarrie)
                //{
                //    OpenDoor(e, "L");
                //    if (currentFee == "BACHA_C14" && isCar == true)
                //        OpenDoor(e, "R");
                //    EventFlag = false;
                //}
                //else
                //{
                //    EventFlag = true;
                //}

                //if (EBS_EatCard(e) == true && msg.MessageType == SystemUI.CardEventType.CARD_DAY.ToString())
                //{
                //    OpenDoor_EBS(e, 1);
                //}

                //sendtoled
                //SendToLed(dspRight, msg.DatetimeIn.ToString("yyyy/MM/dd HH:mm"), msg.DatetimeOut.ToString("yyyy/MM/dd HH:mm"), msg.PlateOut, string.Format("{0:0,0}", msg.Money), msg.CardType, msg.CardState);

                //if (currentFee.Contains("VINCOM") && msg.MessageType == SystemUI.CardEventType.CARD_MONTH.ToString())
                //{
                //    //display customer's image
                //    if (msg.Avatar != "" && msg.Avatar.Split('/').Length == 3)
                //    {
                //        string _piccustomer = StaticPool.PicPathCustomer + @"\" + msg.Avatar.Split('/')[2];
                //        if (File.Exists(_piccustomer))
                //        {
                //            picCustomer.Image = Image.FromFile(_piccustomer);
                //        }
                //    }
                //}

                //if (currentFee.Contains("VINCOM") && msg.MessageType == SystemUI.CardEventType.CARD_DAY.ToString())
                //{
                //    long _ddiff = DateDiff(DateInterval.Day, _msg.DatetimeIn, _msg.DatetimeOut);
                //    if (_ddiff >= 1)
                //    {
                //        IsProcessing = true;
                //        MessageBox.Show("XE ĐỂ QUA ĐÊM", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        IsProcessing = false;
                //    }
                //}

            }
            else
            {
                //picPlateOut.Image = null;
                //picOverViewOut.Image = null;
                ////EventFlag = false;
                //if (msg.CardState != "")
                //{
                //    SendToLed(dspRight, msg.DatetimeIn.ToString("yyyy/MM/dd HH:mm"), msg.DatetimeOut.ToString("yyyy/MM/dd HH:mm"), msg.PlateOut, string.Format("{0:0,0}", msg.Money), msg.CardType, msg.CardState);
                //}
            }

            //OpenDoorTVG(4);

            return msg;
        }

        void ProcessEventOut_PE(string plate, string picdir, DateTime dtime, string currentFee, API_CardInfo cardInfo, tblLane currentLane, User currentUser, ref tblCardEvent currentEvent, ref MessageResult msg)
        {
            //IFee Fee = InterfaceFactory.CreatFee(currentFee);

            int CardMonthExpireDays = 15;
            bool IsHaveMoneyExpiredDate = false;

            msg.Cardnumber = cardInfo.CardNumber;

            //black list
            if (CheckBlackList_PE(plate))
            {
                msg.MessageType = CardEventType.BLACKLIST.ToString();
                msg.IsBlackList = true;
                SaveEventAlarm(dtime, cardInfo.CardNumber, plate, picdir, AlarmType.BLACKLIST, "");
            }

            if (cardInfo.CardGroupName != null)
            {
                msg.CardType = cardInfo.CardType;

                if (cardInfo.CardGroupInactive == true)
                {
                    msg.MessageType = CardEventType.CARD_LOCK.ToString();
                    msg.MessageContent = "NHÓM THẺ BỊ KHÓA";
                    msg.MessageColor = Color.Red;
                    SaveEventAlarm(dtime, cardInfo.CardNumber, plate, picdir, AlarmType.CARDGROUP_LOCK, "");

                    return;
                }
            }

            msg.Cardgroup = cardInfo.CardGroupName;
            msg.CardNo = cardInfo.CardNo;
            if (cardInfo.IsLock)
            {
                msg.MessageType = CardEventType.CARD_LOCK.ToString();
                msg.MessageContent = "THẺ BỊ KHÓA";
                if (!string.IsNullOrWhiteSpace(cardInfo.CardDescription))
                {
                    msg.MessageContent = msg.MessageContent + "-" + cardInfo.CardDescription;
                }
                msg.MessageColor = Color.Red;
                SaveEventAlarm(dtime, cardInfo.CardNumber, plate, picdir, AlarmType.CARD_LOCK, "");

                return;
            }

            //check accesslevel
            if (CheckAccessLevel(cardInfo.CardGroupID, currentLane.LaneID.ToString()) == false)
            {
                msg.MessageType = CardEventType.INACCESSABLE.ToString();
                msg.MessageContent = "SAI QUYỀN TRUY CẬP";
                msg.MessageColor = Color.Red;
                SaveEventAlarm(dtime, cardInfo.CardNumber, plate, picdir, AlarmType.INACCESSABLE, "");
                return;
            }

            msg.CardNo = cardInfo.CardNo;

            //if month card or free card
            msg.PayType = cardInfo.CardGroupName != null ? cardInfo.CardType : -1;

            //access for EBS eatcard
            //if (EBS_Inaccessable(ce, paytype) == true)
            //{
            //    msg.MessageType = CardEventType.INACCESSABLE.ToString();
            //    msg.MessageContent = "SAI ĐẦU ĐỌC THẺ";
            //    msg.MessageColor = Color.Red;
            //    SaveEventAlarm(dtime, cardInfo.CardType.ToString(), plate, picdir, AlarmType.INACCESSABLE, "");
            //    return;
            //}

            var _listEventIn = _API_MobileService.GetEventInByCardNumber(cardInfo.CardNumber);
            if (_listEventIn.Count() == 0)
            {
                msg.MessageType = CardEventType.VEHICLE_NOT_GET_IN.ToString();
                msg.MessageContent = "XE CHƯA VÀO BÃI";
                msg.MessageColor = Color.Red;
                SaveEventAlarm(dtime, cardInfo.CardNumber, plate, picdir, AlarmType.VEHICLE_NOT_GET_IN, "");
                if (currentFee == "VISTA")
                {
                    //OutputByAudio("XeChuaVaoBai.wav");
                    return;
                }

                if (msg.PayType == 0)
                {
                    DateTime dtimeexpire = DateTime.Now.AddDays(-1);//DateTime.Parse(drvcard["ExpireDate"].ToString());

                    if (cardInfo.ExpireDate != null)
                        dtimeexpire = (DateTime)cardInfo.ExpireDate;
                    if (currentFee == "LVT"
                        || currentFee == "AQUA"
                        )
                    {
                        msg.MessageContentEx = "THỜI HẠN THẺ: " + dtimeexpire.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        long _ddiff = DateDiff(DateInterval.Day, dtime, dtimeexpire);
                        if (_ddiff <= CardMonthExpireDays)
                        {
                            if (_ddiff == 0 && dtime > dtimeexpire.AddDays(1))
                            {
                                msg.MessageContentEx = "HẾT HẠN SD" + "-" + dtimeexpire.ToString("dd/MM/yyyy");
                                msg.CardState = "expired";
                            }
                            else if (_ddiff > 0 || (_ddiff == 0 && dtime > dtimeexpire))
                            {
                                msg.MessageContentEx = "THSD THẺ CÒN " + _ddiff.ToString() + " NGÀY";
                                msg.CardState = "underexpired";
                            }
                        }

                        //check registed plate
                        string _plate1 = cardInfo.Plate1 ?? "";
                        string _plate2 = cardInfo.Plate2 ?? "";
                        string _plate3 = cardInfo.Plate3 ?? "";
                        msg.RegistedPlate = _plate1;
                        msg.VehicleName = cardInfo.VehicleName1;
                        if (_plate2 != "")
                            msg.RegistedPlate = msg.RegistedPlate + ";" + _plate2;
                        if (_plate3 != "")
                            msg.RegistedPlate = msg.RegistedPlate + ";" + _plate3;

                        //msg
                        msg.CustomerCode = cardInfo.CustomerCode;
                        msg.CustomerName = cardInfo.CustomerName;
                        msg.Address = cardInfo.Address;
                        msg.IDNumber = cardInfo.IDNumber;
                        msg.Mobile = cardInfo.Mobile;
                        msg.CustomerGroup = cardInfo.CustomerGroupName;


                        var _dsTemp = Data.Event.SqlHelper.ExcuteSQLEvent.GetDataSet("select * from tblCardEvent where EventCode='1' and IsDelete=0" +
                         " and Cardnumber<>'" + cardInfo.CardNumber +
                         "' and PlateIn<>''" +
                         " and (replace(replace(replace(PlateIn,'.',''),'-',''),' ','')='" + _plate1.Replace(".", "").Replace("-", "").Replace(" ", "") +
                         "' or replace(replace(replace(PlateIn,'.',''),'-',''),' ','')='" + _plate2.Replace(".", "").Replace("-", "").Replace(" ", "") +
                         "' or replace(replace(replace(PlateIn,'.',''),'-',''),' ','')='" + _plate3.Replace(".", "").Replace("-", "").Replace(" ", "") +
                         "')"
                         );

                        if (_dsTemp.Tables.Count > 0 && _dsTemp.Tables[0].Rows.Count > 0)
                        {
                            msg.MessageContentEx = "NỢ THẺ LƯỢT-" + _dsTemp.Tables[0].Rows[0]["CardNumber"].ToString();
                        }
                    }
                }

                return;
            }
            else if (_listEventIn.Count() == 1)
            {
                currentEvent = _listEventIn.FirstOrDefault();
                string eventid = currentEvent.Id.ToString();
                DateTime dtimein = dtime;
                if (currentEvent.DatetimeIn != null)
                    dtimein = (DateTime)currentEvent.DatetimeIn;

                msg.PlateIn = currentEvent.PlateIn;

                long _money = 0;


                msg.Cardgroup = cardInfo.CardGroupName;
                msg.Vehicle = cardInfo.VehicleGroupName;

                if (currentFee == "HHT" && (cardInfo.CardGroupID == "78957e8d-aada-44ce-9047-096c4c86ae20" || cardInfo.CardGroupID == "d3d45321-a8b9-4304-bcd8-a6d4852efe0f"))
                {

                    long _mdiffout = DateDiff(DateInterval.Second, dtimein, dtime);
                    if (_mdiffout < 30)
                        return;

                }
                if (currentFee == "LANGHA" ||
                    currentFee == "CUADONG"
                    )
                {
                    long _mdiffout = DateDiff(DateInterval.Second, dtimein, dtime);
                    if (_mdiffout < 50)
                        return;
                }

                if (cardInfo.CardType == 0 || cardInfo.CardType == 2)//month
                {
                    if (cardInfo.CardType == 0)
                    {
                        DateTime dtimeexpire = DateTime.Now.AddDays(-1);//DateTime.Parse(drvcard["ExpireDate"].ToString());
                        if (cardInfo.ExpireDate != null)
                            dtimeexpire = (DateTime)cardInfo.ExpireDate;

                        ////for T&T
                        //if (AcceptCardExpireOut() == false && dtime > dtimeexpire)
                        ////if(currentFee=="TAT"&& dtime > dtimeexpire)
                        //{
                        //    msg.MessageType = CardEventType.CARD_EXPIRE.ToString();
                        //    msg.MessageContent = "HẾT HẠN SD" + "-" + dtimeexpire.ToString("dd/MM/yyyy");
                        //    msg.MessageColor = Color.Red;
                        //    SaveEventAlarm(dtime, cardInfo.CardNumber, plate, picdir, AlarmType.CARD_EXPIRED, "");
                        //    //OutputByAudio("TheHetHan.wav");
                        //    return;
                        //}

                        long _ddiff = DateDiff(DateInterval.Day, dtime, dtimeexpire);
                        if (_ddiff <= CardMonthExpireDays)
                        {
                            if (_ddiff == 0 && dtime > dtimeexpire.AddDays(1))
                            {
                                msg.MessageContentEx = "HẾT HẠN SD" + "-" + dtimeexpire.ToString("dd/MM/yyyy");
                                msg.CardState = "expired";
                            }
                            else if (_ddiff > 0 || (_ddiff == 0 && dtime > dtimeexpire))
                            {
                                msg.MessageContentEx = "THSD THẺ CÒN " + _ddiff.ToString() + " NGÀY";
                                msg.CardState = "underdeadline";
                            }
                        }

                        if (currentFee == "AQUA")
                        {
                            msg.MessageContentEx = "THỜI HẠN THẺ: " + dtimeexpire.ToString("dd/MM/yyyy");
                        }
                    }
                    bool result = false;
                    string _usedtimes = "";
                    _money = FeeCalculate(dtimein, dtime, cardInfo.CardGroupID, cardInfo.ExpireDate.ToString(), ref result, cardInfo.CardNumber, ref _usedtimes);


                    if (_usedtimes != "" && _usedtimes.Contains(";"))
                    {
                        string[] t = _usedtimes.Split(';');
                        if (t != null && t.Length == 4)
                        {
                            msg.UsedTimeDay = int.Parse(t[0]);
                            msg.AvailableTimeDay = int.Parse(t[1]);
                            msg.UsedTimeMonth = int.Parse(t[2]);
                            msg.AvailableTimeMonth = int.Parse(t[3]);
                        }
                    }
                    //check plate: in and out, out and registed

                    bool isAlarm = true;

                    //check registed plate
                    string _plate1 = cardInfo.Plate1 ?? "";
                    string _plate2 = cardInfo.Plate2 ?? "";
                    string _plate3 = cardInfo.Plate3 ?? "";
                    msg.RegistedPlate = _plate1;
                    msg.VehicleName = cardInfo.VehicleName1;
                    if (_plate2 != "")
                        msg.RegistedPlate = msg.RegistedPlate + ";" + _plate2;
                    if (_plate3 != "")
                        msg.RegistedPlate = msg.RegistedPlate + ";" + _plate3;

                    if (currentFee == "CHUNGCU_HVQP")
                    {
                        if (ComparePlate(plate, msg.RegistedPlate, "OUT", cardInfo.VehicleGroupID, currentLane) == true)// && ComparePlate(plate, msg.PlateIn, "OUT", cardgroupID) == true)
                        {
                            isAlarm = false;
                        }
                    }
                    else
                    {
                        if (ComparePlate(plate, msg.RegistedPlate, "OUT", cardInfo.VehicleGroupID, currentLane) == true && ComparePlate(plate, msg.PlateIn, "OUT", cardInfo.VehicleGroupID, currentLane) == true)
                        {
                            isAlarm = false;
                        }
                    }
                    msg.IsAlarm = isAlarm;

                    //msg
                    msg.CustomerCode = cardInfo.CustomerCode;
                    msg.CustomerName = cardInfo.CustomerName;
                    msg.Address = cardInfo.Address;
                    msg.IDNumber = cardInfo.IDNumber;
                    msg.Mobile = cardInfo.Mobile;
                    msg.CustomerGroup = cardInfo.CustomerGroupName;


                    msg.MessageType = CardEventType.CARD_MONTH.ToString();

                    if (isAlarm == false)
                    {

                        msg.MessageContent = "HẸN GẶP LẠI";
                        msg.MessageColor = Color.Lime;
                    }
                    else
                    {
                        msg.MessageContent = "CẢNH BÁO BIỂN SỐ";
                        msg.MessageColor = Color.Red;
                    }

                    if (_money == 0)
                    {
                        if (isAlarm)
                            msg.IsOpenBarrie = false;
                        else
                            msg.IsOpenBarrie = true;
                    }
                    else
                    {
                        msg.IsOpenBarrie = false;
                    }

                    if (isAlarm == true && currentLane.CheckPlateLevelOut == 3)//do not open barrier
                    {
                        msg.MessageContent = "HẸN GẶP LẠI";
                        msg.MessageColor = Color.Yellow;
                        msg.IsOpenBarrie = false;
                    }

                    msg.DatetimeIn = dtimein;
                    msg.DatetimeOut = dtime;

                    msg.PicDirIn = currentEvent.PicDirIn;
                    //msg.PICDIRIN2 = dteventin.Rows[0]["PicDirOut"].ToString();
                    msg.PicDirOut = picdir;

                    msg.PlateOut = plate;
                    // msg.RegistedPlate = dteventin.Rows[0]["RegistedPlate"].ToString();
                    msg.Money = _money;




                    bool bl = SaveCardEventOut(dtime, plate, picdir, _money, currentUser, currentLane, ref currentEvent);
                    if (bl == true)
                    {
                        msg.CardEventOutID = eventid;
                    }
                    else
                    {
                        msg.CardEventOutID = "";
                    }
                    // msg.CardEventInID = msg.CardEventOutID;
                    if (currentFee == "NOIBAI" && msg.Money > 0)
                    {
                        //GetPrintIndex(eventid);
                    }

                    msg.Avatar = cardInfo.Avatar;

                    if (isAlarm && msg.CardEventOutID != "")
                    {
                        //OutputByAudio("coihu.wav");
                    }

                    //if (currentFee == "UDICCOMPLEX" && msg.MessageContentEx.Contains("HẾT HẠN SD"))
                    if (msg.MessageContentEx.Contains("HẾT HẠN SD"))
                    {
                        //OutputByAudio("TheHetHan.wav");
                    }

                    if (currentFee == "PHUTHO")
                        msg.IsOpenBarrie = true;
                    if (currentFee == "BVTMHMT" && cardInfo.VehicleGroupID != "84a002e0-34f3-46da-9b69-1fa76fcf4c91")
                        msg.IsOpenBarrie = false;

                    var _dsTemp = Data.Event.SqlHelper.ExcuteSQLEvent.GetDataSet("select * from tblCardEvent where EventCode='1' and IsDelete=0" +
                     " and Cardnumber<>'" + cardInfo.CardNumber +
                     "' and PlateIn<>''" +
                     " and (replace(replace(replace(PlateIn,'.',''),'-',''),' ','')='" + _plate1.Replace(".", "").Replace("-", "").Replace(" ", "") +
                     "' or replace(replace(replace(PlateIn,'.',''),'-',''),' ','')='" + _plate2.Replace(".", "").Replace("-", "").Replace(" ", "") +
                     "' or replace(replace(replace(PlateIn,'.',''),'-',''),' ','')='" + _plate3.Replace(".", "").Replace("-", "").Replace(" ", "") +
                     "')"
                     );

                    if (_dsTemp.Tables.Count > 0 && _dsTemp.Tables[0].Rows.Count > 0)
                    {
                        msg.MessageContentEx = "NỢ THẺ LƯỢT-" + _dsTemp.Tables[0].Rows[0]["CardNumber"].ToString();
                    }


                }
                else //day
                {
                    bool result = false;
                    string _usedtimes = "";
                    _money = FeeCalculate(dtimein, dtime, cardInfo.CardGroupID, cardInfo.ExpireDate.ToString(), ref result, cardInfo.CardNumber, ref _usedtimes);



                    msg.MessageType = CardEventType.CARD_DAY.ToString();

                    if (plate != "")
                    {
                        string _plate = plate.Replace(" ", "").Replace("-", "").Replace(".", "");

                        var _dsTemp = Data.SqlHelper.ExcuteSQL.GetDataSet("select * from tblCard where IsDelete=0 and IsLock=0 and" +
                        " (replace(replace(replace(Plate1,'.',''),'-',''),' ','')='" + _plate +
                        "' or replace(replace(replace(Plate2,'.',''),'-',''),' ','')='" + _plate +
                        "' or replace(replace(replace(Plate3,'.',''),'-',''),' ','')='" + _plate +
                        "')");

                        if (_dsTemp.Tables.Count > 0 && _dsTemp.Tables[0].Rows.Count > 0)
                        {
                            msg.RegistedPlate = "BS THUÊ BAO";
                        }
                    }

                    bool isAlarm = true;
                    if (ComparePlate(plate, msg.PlateIn, "OUT", cardInfo.VehicleGroupID, currentLane) == true)
                    {
                        isAlarm = false;
                    }
                    if (isAlarm == true)
                    {
                        msg.MessageColor = Color.Yellow;
                    }
                    else
                    {
                        msg.MessageColor = Color.Lime;
                    }
                    msg.IsAlarm = isAlarm;

                    msg.MessageContent = "THU TIỀN";

                    msg.DatetimeIn = dtimein;
                    msg.DatetimeOut = dtime;

                    msg.PicDirIn = currentEvent.PicDirIn;
                    //msg.PICDIRIN2 = dteventin.Rows[0]["PicDirOut"].ToString();
                    msg.PicDirOut = picdir;
                    msg.PlateIn = currentEvent.PlateIn;
                    msg.PlateOut = plate;
                    //msg.RegistedPlate = dteventin.Rows[0]["RegistedPlate"].ToString();
                    msg.Money = _money;
                    msg.IsOpenBarrie = false;
                    msg.MessageContentEx = "";
                    SaveCardEventOut(dtime, plate, picdir, _money, currentUser, currentLane, ref currentEvent);
                    msg.CardEventOutID = eventid;
                    //msg.CardEventInID = msg.CardEventOutID;
                    if (currentFee == "NOIBAI" && msg.Money > 0)
                    {
                        //GetPrintIndex(eventid);
                    }

                    if (currentFee == "PHUTHO" ||
                        currentFee == "VANMIEU" ||
                        currentFee == "CUADONG"
                        )
                        msg.IsOpenBarrie = true;

                    if (currentFee == "BVTMHMT" && cardInfo.VehicleGroupID != "84a002e0-34f3-46da-9b69-1fa76fcf4c91")
                        msg.IsOpenBarrie = false;
                    if (currentFee == "SMARTCARD" && isAlarm == false)
                    {
                        msg.IsOpenBarrie = true;
                    }

                    //if (currentFee == "VANMIEU")
                    //    msg.IsOpenBarrie = true;

                    if (currentFee == "TAK_110CAUGIAY" && _usedtimes != "")
                    {
                        msg.MessageContentEx = _usedtimes;
                        msg.Money = 0;
                    }
                }
            }
            else if (_listEventIn.Count() >= 2)
            {
                string _plate1 = cardInfo.Plate1 ?? "";
                string _plate2 = cardInfo.Plate2 ?? "";
                string _plate3 = cardInfo.Plate3 ?? "";

                bool isexist = false;

                currentEvent = _listEventIn.FirstOrDefault();

                string eventid = currentEvent.Id.ToString();
                DateTime dtimein = dtime;
                if (cardInfo.ExpireDate != null)
                    dtimein = (DateTime)cardInfo.ExpireDate;
                msg.PicDirIn = currentEvent.PicDirIn;
                //msg.PICDIRIN2 = dteventin.Rows[0]["PicDirOut"].ToString();
                msg.PlateIn = currentEvent.PlateIn;

                foreach (var _ev in _listEventIn)
                {
                    if (plate.Replace(".", "").Replace("-", "").Replace(" ", "") == _ev.PlateIn.Replace(".", "").Replace("-", "").Replace(" ", ""))
                    {
                        if (plate.Replace(".", "").Replace("-", "").Replace(" ", "") == _plate1.Replace(".", "").Replace("-", "").Replace(" ", "") ||
                            plate.Replace(".", "").Replace("-", "").Replace(" ", "") == _plate2.Replace(".", "").Replace("-", "").Replace(" ", "") ||
                            plate.Replace(".", "").Replace("-", "").Replace(" ", "") == _plate3.Replace(".", "").Replace("-", "").Replace(" ", "")
                            )
                        {
                            currentEvent = _ev;
                            eventid = _ev.Id.ToString();
                            if (_ev.DatetimeIn != null)
                                dtimein = (DateTime)_ev.DatetimeIn;
                            msg.PicDirIn = _ev.PicDirIn;
                            //msg.PICDIRIN2 = dr["PicDirOut"].ToString();
                            msg.PlateIn = _ev.PlateIn;
                            isexist = true;
                            break;
                        }
                    }
                }

                if (isexist == false)
                {
                    msg.MessageType = CardEventType.INACCESSABLE.ToString();
                    msg.MessageContent = "KHÔNG TÌM THẤY BIỂN SỐ";
                    msg.MessageColor = Color.Red;
                    return;
                }


                long _money = 0;
                msg.Cardgroup = cardInfo.CardGroupName;
                msg.Vehicle = cardInfo.VehicleGroupName;

                if (currentFee == "HHT" && (cardInfo.CardGroupID == "78957e8d-aada-44ce-9047-096c4c86ae20" || cardInfo.CardGroupID == "d3d45321-a8b9-4304-bcd8-a6d4852efe0f"))
                {

                    long _mdiffout = DateDiff(DateInterval.Second, dtimein, dtime);
                    if (_mdiffout < 30)
                        return;

                }
                if (currentFee == "LANGHA" ||
                    currentFee == "CUADONG"
                    )
                {
                    long _mdiffout = DateDiff(DateInterval.Second, dtimein, dtime);
                    if (_mdiffout < 50)
                        return;
                }

                if (cardInfo.CardType == 0 || cardInfo.CardType == 2)//month
                {
                    if (cardInfo.CardType == 0)
                    {


                        DateTime dtimeexpire = DateTime.Now.AddDays(-1);//DateTime.Parse(drvcard["ExpireDate"].ToString());
                        if (cardInfo.ExpireDate != null)
                            dtimeexpire = (DateTime)cardInfo.ExpireDate;

                        ////for T&T
                        //if (AcceptCardExpireOut() == false && dtime > dtimeexpire)
                        ////if(currentFee=="TAT"&& dtime > dtimeexpire)
                        //{
                        //    msg.MessageType = CardEventType.CARD_EXPIRE.ToString();
                        //    msg.MessageContent = "HẾT HẠN SD" + "-" + dtimeexpire.ToString("dd/MM/yyyy");
                        //    msg.MessageColor = Color.Red;
                        //    SaveEventAlarm(dtime, cardInfo.CardNumber, plate, picdir, AlarmType.CARD_EXPIRED, "");
                        //    //OutputByAudio("TheHetHan.wav");
                        //    return;
                        //}

                        long _ddiff = DateDiff(DateInterval.Day, dtime, dtimeexpire);
                        if (_ddiff <= CardMonthExpireDays)
                        {
                            if (_ddiff == 0 && dtime > dtimeexpire.AddDays(1))
                            {
                                msg.MessageContentEx = "HẾT HẠN SD" + "-" + dtimeexpire.ToString("dd/MM/yyyy");
                                msg.CardState = "expired";
                            }
                            else if (_ddiff > 0 || (_ddiff == 0 && dtime > dtimeexpire))
                            {
                                msg.MessageContentEx = "THSD THẺ CÒN " + _ddiff.ToString() + " NGÀY";
                                msg.CardState = "underdeadline";
                            }
                        }

                        if (currentFee == "AQUA")
                        {
                            msg.MessageContentEx = "THỜI HẠN THẺ: " + dtimeexpire.ToString("dd/MM/yyyy");
                        }
                    }
                    bool result = false;
                    string _usedtimes = "";
                    _money = FeeCalculate(dtimein, dtime, cardInfo.CardGroupID, cardInfo.ExpireDate.ToString(), ref result, cardInfo.CardNumber, ref _usedtimes);


                    if (_usedtimes != "" && _usedtimes.Contains(";"))
                    {
                        string[] t = _usedtimes.Split(';');
                        if (t != null && t.Length == 4)
                        {
                            msg.UsedTimeDay = int.Parse(t[0]);
                            msg.AvailableTimeDay = int.Parse(t[1]);
                            msg.UsedTimeMonth = int.Parse(t[2]);
                            msg.AvailableTimeMonth = int.Parse(t[3]);
                        }
                    }
                    //check plate: in and out, out and registed

                    bool isAlarm = true;

                    //check registed plate

                    msg.RegistedPlate = _plate1;
                    msg.VehicleName = cardInfo.VehicleName1;
                    if (_plate2 != "")
                        msg.RegistedPlate = msg.RegistedPlate + ";" + _plate2;
                    if (_plate3 != "")
                        msg.RegistedPlate = msg.RegistedPlate + ";" + _plate3;

                    if (currentFee == "CHUNGCU_HVQP")
                    {
                        if (ComparePlate(plate, msg.RegistedPlate, "OUT", cardInfo.VehicleGroupID, currentLane) == true)// && ComparePlate(plate, msg.PlateIn, "OUT", cardgroupID) == true)
                        {
                            isAlarm = false;
                        }
                    }
                    else
                    {
                        if (ComparePlate(plate, msg.RegistedPlate, "OUT", cardInfo.VehicleGroupID, currentLane) == true && ComparePlate(plate, msg.PlateIn, "OUT", cardInfo.VehicleGroupID, currentLane) == true)
                        {
                            isAlarm = false;
                        }
                    }
                    msg.IsAlarm = isAlarm;

                    //msg
                    msg.CustomerCode = cardInfo.CustomerCode;
                    msg.CustomerName = cardInfo.CustomerName;
                    msg.Address = cardInfo.Address;
                    msg.IDNumber = cardInfo.IDNumber;
                    msg.Mobile = cardInfo.Mobile;
                    msg.CustomerGroup = cardInfo.CustomerGroupName;


                    msg.MessageType = CardEventType.CARD_MONTH.ToString();

                    if (isAlarm == false)
                    {

                        msg.MessageContent = "HẸN GẶP LẠI";
                        msg.MessageColor = Color.Lime;
                    }
                    else
                    {
                        msg.MessageContent = "CẢNH BÁO BIỂN SỐ";
                        msg.MessageColor = Color.Red;
                    }

                    if (_money == 0)
                    {
                        if (isAlarm)
                            msg.IsOpenBarrie = false;
                        else
                            msg.IsOpenBarrie = true;
                    }
                    else
                    {
                        msg.IsOpenBarrie = false;
                    }

                    if (isAlarm == true && currentLane.CheckPlateLevelOut == 3)//do not open barrier
                    {
                        msg.MessageContent = "HẸN GẶP LẠI";
                        msg.MessageColor = Color.Yellow;
                        msg.IsOpenBarrie = false;
                    }

                    msg.DatetimeIn = dtimein;
                    msg.DatetimeOut = dtime;


                    msg.PicDirOut = picdir;

                    msg.PlateOut = plate;
                    // msg.RegistedPlate = dteventin.Rows[0]["RegistedPlate"].ToString();
                    msg.Money = _money;




                    bool bl = SaveCardEventOut(dtime, plate, picdir, _money, currentUser, currentLane, ref currentEvent);
                    if (bl == true)
                    {
                        msg.CardEventOutID = eventid;
                    }
                    else
                    {
                        msg.CardEventOutID = "";
                    }
                    // msg.CardEventInID = msg.CardEventOutID;
                    if (currentFee == "NOIBAI" && msg.Money > 0)
                    {
                        //GetPrintIndex(eventid);
                    }

                    msg.Avatar = cardInfo.Avatar;

                    if (isAlarm && msg.CardEventOutID != "")
                    {
                        //OutputByAudio("coihu.wav");
                    }

                    //if (currentFee == "UDICCOMPLEX" && msg.MessageContentEx.Contains("HẾT HẠN SD"))
                    if (msg.MessageContentEx.Contains("HẾT HẠN SD"))
                    {
                        //OutputByAudio("TheHetHan.wav");
                    }

                    if (currentFee == "PHUTHO")
                        msg.IsOpenBarrie = true;
                    if (currentFee == "BVTMHMT" && cardInfo.VehicleGroupID != "84a002e0-34f3-46da-9b69-1fa76fcf4c91")
                        msg.IsOpenBarrie = false;

                    var _dsTemp = Data.Event.SqlHelper.ExcuteSQLEvent.GetDataSet("select * from tblCardEvent where EventCode='1' and IsDelete=0" +
                           " and Cardnumber<>'" + cardInfo.CardNumber +
                           "' and PlateIn<>''" +
                           " and (replace(replace(replace(PlateIn,'.',''),'-',''),' ','')='" + _plate1.Replace(".", "").Replace("-", "").Replace(" ", "") +
                           "' or replace(replace(replace(PlateIn,'.',''),'-',''),' ','')='" + _plate2.Replace(".", "").Replace("-", "").Replace(" ", "") +
                           "' or replace(replace(replace(PlateIn,'.',''),'-',''),' ','')='" + _plate3.Replace(".", "").Replace("-", "").Replace(" ", "") +
                           "')"
                           );

                    if (_dsTemp.Tables.Count > 0 && _dsTemp.Tables[0].Rows.Count > 0)
                    {
                        msg.MessageContentEx = "NỢ THẺ LƯỢT-" + _dsTemp.Tables[0].Rows[0]["CardNumber"].ToString();
                    }
                }
                else //day
                {
                    bool result = false;
                    string _usedtimes = "";
                    _money = FeeCalculate(dtimein, dtime, cardInfo.CardGroupID, cardInfo.ExpireDate.ToString(), ref result, cardInfo.CardNumber, ref _usedtimes);



                    msg.MessageType = CardEventType.CARD_DAY.ToString();

                    if (plate != "")
                    {
                        string _plate = plate.Replace(" ", "").Replace("-", "").Replace(".", "");

                        var _dsTemp = Data.SqlHelper.ExcuteSQL.GetDataSet("select * from tblCard where IsDelete=0 and IsLock=0 and" +
                        " (replace(replace(replace(Plate1,'.',''),'-',''),' ','')='" + _plate +
                        "' or replace(replace(replace(Plate2,'.',''),'-',''),' ','')='" + _plate +
                        "' or replace(replace(replace(Plate3,'.',''),'-',''),' ','')='" + _plate +
                        "')");

                        if (_dsTemp.Tables.Count > 0 && _dsTemp.Tables[0].Rows.Count > 0)
                        {
                            msg.RegistedPlate = "BS THUÊ BAO";
                        }
                    }

                    bool isAlarm = true;
                    if (ComparePlate(plate, msg.PlateIn, "OUT", cardInfo.VehicleGroupID, currentLane) == true)
                    {
                        isAlarm = false;
                    }
                    if (isAlarm == true)
                    {
                        msg.MessageColor = Color.Yellow;
                    }
                    else
                    {
                        msg.MessageColor = Color.Lime;
                    }
                    msg.IsAlarm = isAlarm;

                    msg.MessageContent = "THU TIỀN";

                    msg.DatetimeIn = dtimein;
                    msg.DatetimeOut = dtime;

                    msg.PicDirIn = currentEvent.PicDirIn;
                    //msg.PICDIRIN2 = dteventin.Rows[0]["PicDirOut"].ToString();
                    msg.PicDirOut = picdir;
                    msg.PlateIn = currentEvent.PlateIn;
                    msg.PlateOut = plate;
                    //msg.RegistedPlate = dteventin.Rows[0]["RegistedPlate"].ToString();
                    msg.Money = _money;
                    msg.IsOpenBarrie = false;
                    msg.MessageContentEx = "";
                    SaveCardEventOut(dtime, plate, picdir, _money, currentUser, currentLane, ref currentEvent);
                    msg.CardEventOutID = eventid;
                    //msg.CardEventInID = msg.CardEventOutID;
                    if (currentFee == "NOIBAI" && msg.Money > 0)
                    {
                        //GetPrintIndex(eventid);
                    }

                    if (currentFee == "PHUTHO" ||
                        currentFee == "VANMIEU" ||
                        currentFee == "CUADONG"
                        )
                        msg.IsOpenBarrie = true;

                    if (currentFee == "BVTMHMT" && cardInfo.VehicleGroupID != "84a002e0-34f3-46da-9b69-1fa76fcf4c91")
                        msg.IsOpenBarrie = false;
                    if (currentFee == "SMARTCARD" && isAlarm == false)
                    {
                        msg.IsOpenBarrie = true;
                    }

                    //if (currentFee == "VANMIEU")
                    //    msg.IsOpenBarrie = true;

                    if (currentFee == "TAK_110CAUGIAY" && _usedtimes != "")
                    {
                        msg.MessageContentEx = _usedtimes;
                        msg.Money = 0;
                    }

                }
            }

            //Display on LED
            //Print
        }

        bool CheckBlackList_PE(string plate2)
        {
            var _check = _API_MobileService.GetCurrentBlackListByPlate(plate2);

            if (_check != null)
            {
                return true;
            }
            return false;
        }

        bool CheckBlackList_VINCOM(string plate, ref string description)
        {
            var _check = _API_MobileService.GetCurrentBlackListByPlate(plate);

            if (_check != null)
            {
                description = _check.Description;
                return true;
            }
            return false;
        }

        bool CreateCardEventIn(DateTime dtimein, string cardnumber2, string platein, string picdirin, string customername, string registedplate, string cardgroupID, string vehiclegroupID, string customergroupID, bool isblacklist, string cardno, string userid, string laneid, ref tblCardEvent currentEvent)
        {
            currentEvent = new tblCardEvent()
            {
                Id = Guid.NewGuid(),
                EventCode = "1",
                CardNumber = cardnumber2 ?? "",
                PlateIn = platein ?? "",
                DatetimeIn = dtimein,
                PicDirIn = picdirin ?? "",
                CustomerName = customername ?? "",
                RegistedPlate = registedplate ?? "",
                CardGroupID = cardgroupID ?? "",
                VehicleGroupID = vehiclegroupID ?? "",
                CustomerGroupID = customergroupID ?? "",
                IsBlackList = isblacklist,
                UserIDIn = userid ?? "",
                LaneIDIn = laneid ?? "",
                CardNo = cardno ?? "",
                LaneIDOut = "",
                UserIDOut = "",
                PlateOut = "",
                FreeType = "",
                IsDelete = false,
                IsFree = false,
                Moneys = 0
            };

            return _API_MobileService.CreateEventIn(currentEvent).isSuccess;
        }

        bool SaveCardEventOut(DateTime dtimeout, string plateout, string picdirout, long money, User currentUser, tblLane currentLane, ref tblCardEvent currentEvent)
        {
            currentEvent.DateTimeOut = dtimeout;
            currentEvent.PlateOut = plateout;
            currentEvent.PicDirOut = picdirout;
            currentEvent.Moneys = money;
            currentEvent.UserIDOut = currentUser.Id.ToString();
            currentEvent.LaneIDOut = currentLane.LaneID.ToString();
            currentEvent.EventCode = "2";

            return _API_MobileService.UpdateEvent(currentEvent).isSuccess;
        }

        public void SaveEventAlarm(DateTime dtime, string cardnumber, string plate, string picdir, AlarmType alarmcode, string description)
        {
            int _code = (int)alarmcode;

            var _alarm = new tblAlarm()
            {
                Date = dtime,
                CardNumber = cardnumber,
                Plate = plate,
                PicDir = picdir,
                AlarmCode = _code.ToString("000"),
                UserID = "",
                LaneID = "",
                Description = description
            };

            _API_MobileService.CreateAlarm(_alarm);
        }

        public long DateDiff(DateInterval interval, DateTime date1, DateTime date2)
        {
            TimeSpan ts = date2.Subtract(date1);
            int datediff = 0;
            for (int i = 1; i < 5000; i++)
            {
                if (date1.AddDays(i).ToString("yyyy/MM/dd") == date2.ToString("yyyy/MM/dd"))
                {
                    datediff = i;
                    break;
                }
            }
            switch (interval)
            {
                case DateInterval.Year:
                    return date2.Year - date1.Year;
                case DateInterval.Month:
                    return (date2.Month - date1.Month) + (12 * (date2.Year - date1.Year));
                case DateInterval.Weekday:
                    return datediff / 7;
                case DateInterval.Day:
                    return datediff; // Fix(ts.TotalDays);
                case DateInterval.Hour:
                    return Fix(ts.TotalHours);
                case DateInterval.Minute:
                    return Fix(ts.TotalMinutes);
                default:
                    return Fix(ts.TotalSeconds);
            }
        }

        public long Fix(double Number)
        {
            if (Number >= 0)
            {
                return (long)Math.Floor(Number);
            }
            return (long)Math.Ceiling(Number);
        }

        public string CreatFileNameIn(DateTime dtime)
        {
            try
            {
                string path = $@"\\{Environment.MachineName}\PIC" + @"\" + dtime.Day.ToString("00") + "-" + dtime.Month.ToString("00") + "-" + dtime.Year.ToString("0000");
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
                Guid g = Guid.NewGuid();

                string filename = dtime.Hour.ToString("00") + "h" + dtime.Minute.ToString("00") + "m" + dtime.Second.ToString("00") + "s_" + g.ToString();

                filename = path + @"\" + filename;

                return filename;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string CreatFileNameOut(DateTime dtime)
        {
            try
            {
                string path = $@"\\{Environment.MachineName}\PICRA" + @"\" + dtime.Day.ToString("00") + "-" + dtime.Month.ToString("00") + "-" + dtime.Year.ToString("0000");
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
                Guid g = Guid.NewGuid();

                string filename = dtime.Hour.ToString("00") + "h" + dtime.Minute.ToString("00") + "m" + dtime.Second.ToString("00") + "s_" + g.ToString();

                filename = path + @"\" + filename;

                return filename;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";
        }

        private bool CheckAccessLevel(string cardgroupID, string laneID)
        {
            var currentCardGroup = _API_MobileService.GetCardgroupById(cardgroupID);
            if (currentCardGroup != null && currentCardGroup.LaneIDs.Contains(laneID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SavePicture(Bitmap img, string picpath)
        {
            try
            {
                if (img != null)
                {
                    img.Save(picpath, ImageFormat.Jpeg);
                }
            }
            catch (Exception ext)
            {
                var t = ext;
            }
        }

        public bool ComparePlate(string plate1, string plate2, string actions, string _vehiclegroupId, tblLane currentLane)
        {

            string Car_VehicleGroupID = "84a002e0-34f3-46da-9b69-1fa76fcf4c91";
            try
            {
                //CardGroup cardgroup = StaticPool.cardgroups.GetCardGroupByCardGroupID(cardgroupid);
                //string _vehiclegroupId = "";
                //if (cardgroup != null)
                //{
                //    if (cardgroup.IsCheckPlate == false)
                //        return true;
                //    _vehiclegroupId = cardgroup.VehicleGroupID;
                //}

                string _plate1 = plate1.Replace(" ", "").Replace("-", "").Replace(".", "");
                string _plate2 = plate2.Replace(" ", "").Replace("-", "").Replace(".", "");

                if (actions == "IN")// && StaticPool.IsCheckPlateIn == false)
                {
                    if (currentLane.CheckPlateLevelIn == 3)
                        return false;

                    if (currentLane.CheckPlateLevelIn == 0)
                        return true;


                    if (_plate1 == "" || _plate2 == "" || _plate1.Length < 4 || _plate2.Length < 4)
                        return false;

                    if (currentLane.CheckPlateLevelIn == 1)
                    {
                        if (_plate1.Contains(_plate2) == true || _plate2.Contains(_plate1) == true)
                        {
                            return true;
                        }
                        else
                        {
                            if (_vehiclegroupId != Car_VehicleGroupID)
                            {
                                string[] _temp1 = _plate1.Split(';');
                                if (_temp1 != null && _temp1.Length > 0)
                                {
                                    for (int i = 0; i < _temp1.Length; i++)
                                    {
                                        string rp1 = "", rp2 = "";
                                        bool bl = SubCompare(_temp1[i], _plate2, ref rp1, ref rp2);
                                        if (bl == true)
                                            return true;
                                    }
                                }

                                string[] _temp2 = _plate2.Split(';');
                                if (_temp2 != null && _temp2.Length > 0)
                                {
                                    for (int i = 0; i < _temp2.Length; i++)
                                    {
                                        string rp1 = "", rp2 = "";
                                        bool bl = SubCompare(_temp2[i], _plate1, ref rp1, ref rp2);
                                        if (bl == true)
                                            return true;
                                    }
                                }
                            }

                            return false;
                        }
                    }
                    else if (currentLane.CheckPlateLevelIn == 2)
                    {
                        string[] _temp1 = _plate1.Split(';');
                        if (_temp1 != null && _temp1.Length > 0)
                        {
                            for (int i = 0; i < _temp1.Length; i++)
                            {
                                if (_temp1[i] == _plate2)
                                    return true;
                            }
                        }

                        string[] _temp2 = _plate2.Split(';');
                        if (_temp2 != null && _temp2.Length > 0)
                        {
                            for (int i = 0; i < _temp2.Length; i++)
                            {
                                if (_temp2[i] == _plate1)
                                    return true;
                            }
                        }
                    }


                }
                else if (actions == "OUT")
                {
                    if (currentLane.CheckPlateLevelOut == 3)
                        return false;

                    if (currentLane.CheckPlateLevelOut == 0)
                        return true;

                    if (_plate1 == "" || _plate2 == "" || _plate1.Length < 4 || _plate2.Length < 4)
                        return false;

                    if (currentLane.CheckPlateLevelOut == 1)
                    {
                        if (_plate1.Contains(_plate2) == true || _plate2.Contains(_plate1) == true)
                        {
                            return true;
                        }
                        else
                        {
                            if (_vehiclegroupId != Car_VehicleGroupID)
                            {
                                string[] _temp1 = _plate1.Split(';');
                                if (_temp1 != null && _temp1.Length > 0)
                                {
                                    for (int i = 0; i < _temp1.Length; i++)
                                    {
                                        string rp1 = "", rp2 = "";
                                        bool bl = SubCompare(_temp1[i], _plate2, ref rp1, ref rp2);
                                        if (bl == true)
                                            return true;
                                    }
                                }

                                string[] _temp2 = _plate2.Split(';');
                                if (_temp2 != null && _temp2.Length > 0)
                                {
                                    for (int i = 0; i < _temp2.Length; i++)
                                    {
                                        string rp1 = "", rp2 = "";
                                        bool bl = SubCompare(_temp2[i], _plate1, ref rp1, ref rp2);
                                        if (bl == true)
                                            return true;
                                    }
                                }
                            }
                            return false;
                        }
                    }
                    else if (currentLane.CheckPlateLevelOut == 2)
                    {
                        string[] _temp1 = _plate1.Split(';');
                        if (_temp1 != null && _temp1.Length > 0)
                        {
                            for (int i = 0; i < _temp1.Length; i++)
                            {
                                if (_temp1[i] == _plate2)
                                    return true;
                            }
                        }

                        string[] _temp2 = _plate2.Split(';');
                        if (_temp2 != null && _temp2.Length > 0)
                        {
                            for (int i = 0; i < _temp2.Length; i++)
                            {
                                if (_temp2[i] == _plate1)
                                    return true;
                            }
                        }
                    }

                }


            }
            catch
            { }
            return false;
        }

        public bool SubCompare(string plate1, string plate2, ref string rP1, ref string rP2)
        {
            try
            {
                //last 5 digit
                string _plate1 = plate1;
                string _plate2 = plate2;
                if (_plate1.Length >= 5)
                    _plate1 = _plate1.Substring(_plate1.Length - 5);
                if (_plate2.Length >= 5)
                    _plate2 = _plate2.Substring(_plate2.Length - 5);
                if (_plate1.Contains(_plate2) || _plate2.Contains(_plate1))
                {
                    rP1 = _plate1;
                    rP2 = _plate2;
                    return true;
                }
                if (_plate1.Length == _plate2.Length)
                {
                    for (int i = 0; i < _plate1.Length; i++)
                    {
                        string _p1 = _plate1.Remove(i, 1);
                        string _p2 = _plate2.Remove(i, 1);
                        if (_p1 == _p2 || _plate1.Contains(_p2) || _plate2.Contains(_p1))
                        {
                            rP1 = _p1;
                            rP2 = _p2;
                            return true;
                        }
                    }
                }
                //if(_plate1.Length>=4)
                //    _plate1= _plate1.Substring(_plate1.Length - 4);
                //if (_plate2.Length >= 4)
                //    _plate2 = _plate2.Substring(_plate2.Length - 4);

                //if (_plate1.Contains(_plate2) || _plate2.Contains(_plate1))
                //{
                //    rP1 = _plate1;
                //    rP2 = _plate2;
                //    return true;
                //}
                //if (_plate1.Length == _plate2.Length)
                //{
                //    for (int i = 0; i < _plate1.Length; i++)
                //    {
                //        string _p1 = _plate1.Remove(i, 1);
                //        string _p2 = _plate2.Remove(i, 1);
                //        if (_p1 == _p2 || _plate1.Contains(_p2) || _plate2.Contains(_p1))
                //        {
                //            rP1 = _p1;
                //            rP2 = _p2;
                //            return true;
                //        }
                //    }
                //}


            }
            catch
            {

            }
            return false;
        }

        public long FeeCalculate(DateTime datetimein, DateTime datetimeout, string cardgroupID, string expiredate, ref bool result, string cardnumber, ref string usedtimes)
        {
            long money = 0;
            result = true;
            try
            {
                var currentCardGroup = _API_MobileService.GetCardgroupById(cardgroupID);
                if (currentCardGroup != null)
                {
                    if (currentCardGroup.CardType == 2)
                        return 0;

                    if (!currentCardGroup.IsHaveMoneyExcessTime && !currentCardGroup.IsHaveMoneyExpiredDate && currentCardGroup.CardType == 0)
                        return 0;
                    else
                    {
                        if (currentCardGroup.IsHaveMoneyExpiredDate)
                        {
                            if (expiredate != "")
                            {
                                if (datetimein < DateTime.Parse(expiredate))
                                {
                                    datetimein = DateTime.Parse(expiredate).AddDays(1);
                                }
                            }
                        }
                    }

                    if (currentCardGroup.EnableFree)
                    {
                        long _mdif = DateDiff(DateInterval.Minute, datetimein, datetimeout);
                        if (_mdif <= currentCardGroup.FreeTime)
                            return 0;
                        else
                        {
                            datetimein = datetimein.AddMinutes(currentCardGroup.FreeTime);
                        }
                    }

                    int mdiff = (int)DateDiff(DateInterval.Minute, datetimein, datetimeout);
                    int ddiff = (int)DateDiff(DateInterval.Day, datetimein, datetimeout);


                    DateTime DayTimeFrom = Convert.ToDateTime(currentCardGroup.DayTimeFrom);
                    DateTime DayTimeTo = Convert.ToDateTime(currentCardGroup.DayTimeTo);


                    int EachFee = currentCardGroup.EachFee;

                    int[] MoneyBlock = new int[6]
                    {
                        currentCardGroup.Block0,
                        currentCardGroup.Block1,
                        currentCardGroup.Block2,
                        currentCardGroup.Block3,
                        currentCardGroup.Block4,
                        currentCardGroup.Block5
                    };
                    int[] TimeBlock = new int[6]
                    {
                        currentCardGroup.Time0,
                        currentCardGroup.Time1,
                        currentCardGroup.Time2,
                        currentCardGroup.Time3,
                        currentCardGroup.Time4,
                        currentCardGroup.Time5,
                    };

                    string TimePeriods = currentCardGroup.TimePeriods;
                    string Costs = currentCardGroup.Costs;

                    int Formulation = currentCardGroup.Formulation;

                    int VehicleType = 0;

                    //DataTable dtvehiclegroup = StaticPool.mdb.FillData("select VehicleType from tblVehicleGroup where VehicleGroupID='" + drv["VehicleGroupID"].ToString() + "'");
                    //if (dtvehiclegroup != null && dtvehiclegroup.Rows.Count > 0)
                    //{
                    //    VehicleType = int.Parse(dtvehiclegroup.Rows[0]["VehicleType"].ToString());
                    //}


                    if (currentCardGroup.CardType == 1)//Day
                    {
                        if (Formulation == 0)//each
                        {
                            money = EachFee;
                        }
                        else if (Formulation == 1)//block
                        {
                            money = MoneyBlock[0] + SubCalculate(mdiff - TimeBlock[0], TimeBlock[1], MoneyBlock[1]);
                        }
                        else if (Formulation == 2)//period
                        {
                            money = CalculateByPeriod(datetimein, datetimeout, TimePeriods.Split('-'), Costs.Split('-'));
                        }
                    }
                    else if (currentCardGroup.CardType == 0 && (currentCardGroup.IsHaveMoneyExcessTime == true || currentCardGroup.IsHaveMoneyExpiredDate == true))//Month
                    {
                        if (currentCardGroup.IsHaveMoneyExpiredDate == true)
                        {
                            if (Formulation == 0)
                            {
                                money = EachFee;
                            }
                        }
                    }

                    return money;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return 0;
        }

        private int SubCalculate(int time, int timeBlock, int moneyBlock)
        {
            int temp = 0;
            if (timeBlock == 0 || time <= 0)
            {
                return 0;
            }
            else
            {
                temp = (time / timeBlock + (time % timeBlock == 0 ? 0 : 1)) * moneyBlock;
            }
            return temp;
        }

        private int CalculateByPeriod(DateTime dtin, DateTime dtout, string[] period, string[] cost)
        {
            try
            {

                if (period == null || cost == null || period.Length == 0 || cost.Length == 0 || (period.Length - cost.Length) != 1)
                {
                    //SystemUI.SaveLogFile("CalculateByPeriod: input data failed");
                    return 0;
                }
                int[] temp = new int[period.Length - 1];
                for (int i = 0; i < temp.Length; i++)
                    temp[i] = 0;
                TimeRecognized[] T = new TimeRecognized[period.Length - 1];
                for (int i = 0; i < T.Length; i++)
                {
                    TimeRecognized _t = new TimeRecognized();
                    T[i] = _t;
                }

                for (DateTime dtime = dtin; dtime <= dtout; dtime = dtime.AddMinutes(1))
                {
                    for (int i = 0; i < period.Length - 1; i++)
                    {

                        if (dtime >= GetDateTime(dtime, DateTime.Parse(period[i])) && dtime < GetDateTime(dtime, DateTime.Parse(period[i + 1])) ||
                            (i == (period.Length - 2) && (dtime >= GetDateTime(dtime, DateTime.Parse(period[i])) || dtime < GetDateTime(dtime, DateTime.Parse(period[i + 1])))))
                        {
                            if (T[i].HaveOverPoint == "")
                            {
                                T[i].Times++;
                                if (i == (period.Length - 2))
                                {

                                    if (dtime >= GetDateTime(dtime, DateTime.Parse(period[i])))
                                    {
                                        T[i].HaveOverPoint = GetDateTime(dtime.AddDays(1), Convert.ToDateTime(period[i + 1])).ToString("yyyy/MM/dd HH:mm");
                                    }
                                    else if (dtime < GetDateTime(dtime, DateTime.Parse(period[i + 1])))
                                    {
                                        T[i].HaveOverPoint = GetDateTime(dtime, Convert.ToDateTime(period[i + 1])).ToString("yyyy/MM/dd HH:mm");
                                    }
                                }
                                else
                                {
                                    T[i].HaveOverPoint = GetDateTime(dtime, Convert.ToDateTime(period[i + 1])).ToString("yyyy/MM/dd HH:mm");
                                }
                            }
                            else
                            {

                                if (dtime > DateTime.Parse(T[i].HaveOverPoint))
                                {
                                    T[i].Times++;
                                    if (i == (period.Length - 2))
                                    {
                                        if (dtime >= GetDateTime(dtime, DateTime.Parse(period[i])))
                                        {
                                            T[i].HaveOverPoint = GetDateTime(dtime.AddDays(1), Convert.ToDateTime(period[i + 1])).ToString("yyyy/MM/dd HH:mm");
                                        }
                                        else if (dtime < GetDateTime(dtime, DateTime.Parse(period[i + 1])))
                                        {
                                            T[i].HaveOverPoint = GetDateTime(dtime, Convert.ToDateTime(period[i + 1])).ToString("yyyy/MM/dd HH:mm");
                                        }
                                    }
                                    else
                                    {
                                        T[i].HaveOverPoint = GetDateTime(dtime, Convert.ToDateTime(period[i + 1])).ToString("yyyy/MM/dd HH:mm");
                                    }
                                }

                            }
                        }
                    }
                }

                int _money = 0;

                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = T[i].Times;
                    _money = _money + temp[i] * int.Parse(cost[i]);
                }

                return _money;


                //return temp;

            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                //SystemUI.SaveLogFile("CalculateByPeriod:" + ex.Message);
            }
            return 0;
        }

        public class TimeRecognized
        {
            public TimeRecognized()
            { }

            private int times = 0;
            public int Times
            {
                get { return times; }
                set { times = value; }
            }

            string countpoint = "";
            public string CountPoint
            {
                get { return countpoint; }
                set { countpoint = value; }
            }

            string haveoverpoint = "";//diem phai qua
            public string HaveOverPoint
            {
                get { return haveoverpoint; }
                set { haveoverpoint = value; }
            }
        }

        public class MessageResult
        {
            public DateTime DatetimeIn { get; set; } = DateTime.Now;
            public DateTime DatetimeOut { get; set; } = DateTime.Now;
            public string PlateIn { get; set; } = "";
            public string PlateOut { get; set; } = "";
            public string Cardnumber { get; set; } = "";
            public string CardNo { get; set; } = "";
            public string RegistedPlate { get; set; } = "";
            public string CustomerName { get; set; } = "";
            public string Address { get; set; } = "";
            public string Vehicle { get; set; } = "";
            public string VehicleName { get; set; } = "";
            public string Cardgroup { get; set; } = "";
            public long Money { get; set; } = 0;
            public bool IsOpenBarrie { get; set; } = false;
            public string MessageType { get; set; } = "";
            public string MessageContent { get; set; } = "";
            public Color MessageColor { get; set; } = Color.Lime;
            public bool CardEventFlag { get; set; } = false;
            public bool LoopEventFlag { get; set; } = false;
            public string CardEventInID { get; set; } = "";
            public string CardEventOutID { get; set; } = "";
            public string MessageContentEx { get; set; } = "";
            public bool IsBlackList { get; set; } = false;
            public string BlackListContent { get; set; } = "";
            public bool IsEditPlateIn { get; set; } = false;
            public bool IsEditPlateOut { get; set; } = false;
            public string LoopEventID { get; set; } = "";
            public string PicDirIn { get; set; } = "";
            public string PICDIRIN2 { get; set; } = "";
            public string PicDirOut { get; set; } = "";
            public string Mobile { get; set; } = "";
            public string CustomerCode { get; set; } = "";
            public string IDNumber { get; set; } = "";
            public string CustomerGroup { get; set; } = "";
            public string Avatar { get; set; } = "";
            //vip, month, day
            public int CardType { get; set; } = -1;
            public int UsedTimeDay { get; set; } = 0;
            public int UsedTimeMonth { get; set; } = 0;
            public int AvailableTimeDay { get; set; } = 0;
            public int AvailableTimeMonth { get; set; } = 0;
            public int PayType { get; set; } = -1;
            public string HavePayMoney { get; set; } = "";
            public bool IsAlarm { get; set; } = true;
            // "":no free, "ALL":free all, "PART":free one part
            public string FreeType { get; set; }
            public string Voucher { get; set; }
            // state= expired - cardexpired
            // state= underdeadline
            public string CardState { get; set; } = "";
        }

        private DateTime GetDateTime(DateTime dtime1, DateTime dtime2)
        {
            try
            {
                string st = dtime1.ToString("yyyy/MM/dd") + " " + dtime2.ToString("HH:mm");
                return Convert.ToDateTime(dtime1.ToString("yyyy/MM/dd") + " " + dtime2.ToString("HH:mm"));
            }
            catch (Exception ex)
            {
                //SystemUI.SaveLogFile("GetDateTime:" + ex.Message);
            }
            return dtime1;
        }

        public class CamControl
        {
            public bool Visible { get; set; }

            public string GetPlate(ref Bitmap pic1, ref Bitmap pic2)
            {
                return "";
            }

            public Image GetCurrentImage()
            {
                return null;
            }
        }

        public enum AlarmType
        {
            INVALID_CARD = 1,
            CARD_LOCK = 2,
            INACCESSABLE = 3,
            VEHICLE_GOT_IN = 4,
            VEHICLE_NOT_GET_IN = 5,
            OPEN_BARRIE_BY_PC = 6,
            OPEN_BARRIE_BY_BUTTON = 7,
            ESCAPE_EVENT_IN = 8,
            ESCAPE_EVENT_OUT = 9,
            CARD_EXPIRED = 10,
            INVALID_PLATE = 11,
            BLACKLIST = 12,
            PLATE_LOCK = 13,
            PLATE_EXPIRE = 14,
            CARDGROUP_LOCK = 15,
            PLATE_NOT_SAME = 16,
            VEHICLE_ON_LOOP = 17
        }

        public enum CardEventType
        {
            INVALID_CARD = 0,
            CARD_LOCK,
            INACCESSABLE,
            VEHICLE_GOT_IN,
            CARD_EXPIRE,
            CARD_DAY,
            CARD_MONTH,
            VEHICLE_NOT_GET_IN,
            BLACKLIST

        }

        public enum DateInterval
        {
            Year,
            Month,
            Weekday,
            Day,
            Hour,
            Minute,
            Second
        }
    }
}
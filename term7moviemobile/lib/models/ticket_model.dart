import 'package:term7moviemobile/models/seat_model.dart';
import 'package:term7moviemobile/models/showtime_model.dart';
import 'package:term7moviemobile/models/ticket_type_model.dart';

class TicketModel {
  int? id;
  String? lockedTime;
  double? receivePrice;
  double? sellingPrice;
  String? showStartTime;
  int? showTimeId;
  ShowtimeModel? showtime;
  String? showtimeTicketTypeId;
  int? statusId;
  String? statusName;
  TicketTypeModel? ticketType;
  SeatModel? seat;

  TicketModel(
      {this.id,
      this.lockedTime,
      this.receivePrice,
      this.sellingPrice,
      this.showStartTime,
      this.showTimeId,
      this.showtimeTicketTypeId,
      this.statusId,
      this.statusName,
      this.ticketType,
      this.seat,
      this.showtime});

  TicketModel.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    lockedTime = json['lockedTime'];
    receivePrice = json['receivePrice'];
    sellingPrice = json['sellingPrice'];
    showStartTime = json['showStartTime'];
    showTimeId = json['showTimeId'];
    showtimeTicketTypeId = json['showtimeTicketTypeId'];
    statusId = json['statusId'];
    statusName = json['statusName'];
    if (json['ticketType'] != null) {
      ticketType = TicketTypeModel.fromJson(json['ticketType']);
    }
    if (json['seat'] != null) {
      seat = SeatModel.fromJson(json['seat']);
    }
    if (json['showtime'] != null) {
      showtime = ShowtimeModel.fromJson(json['showtime']);
    }
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['id'] = this.id;
    data['lockedTime'] = this.lockedTime;
    data['receivePrice'] = this.receivePrice;
    data['sellingPrice'] = this.sellingPrice;
    data['showStartTime'] = this.showStartTime;
    data['showTimeId'] = this.showTimeId;
    data['showtimeTicketTypeId'] = this.showtimeTicketTypeId;
    data['statusId'] = this.statusId;
    data['statusName'] = this.statusName;
    if (this.ticketType != null) {
      data['ticketType'] = this.ticketType!.toJson();
    }
    if (this.seat != null) {
      data['seat'] = this.seat!.toJson();
    }
    if (this.showtime != null) {
      data['showtime'] = this.showtime!.toJson();
    }
    return data;
  }
}

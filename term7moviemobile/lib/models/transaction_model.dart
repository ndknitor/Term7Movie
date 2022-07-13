import 'package:term7moviemobile/models/showtime_model.dart';
import 'package:term7moviemobile/models/theater_model.dart';
import 'package:term7moviemobile/models/ticket_model.dart';

class TransactionModel {
  String? transactionId;
  ShowtimeModel? showtime;
  TheaterModel? theater;
  String? purchasedDate;
  String? validUntil;
  int? statusId;
  String? statusName;
  String? qrCodeUrl;
  double? total;
  List<TicketModel>? tickets;

  TransactionModel(
      {this.transactionId,
      this.showtime,
      this.tickets,
      this.total,
      this.purchasedDate,
      this.qrCodeUrl,
      this.statusId,
      this.statusName,
      this.theater,
      this.validUntil,});

  TransactionModel.fromJson(Map<String, dynamic> json) {
    transactionId = json['id'];
    if (json['showtime'] != null) {
      showtime = ShowtimeModel.fromJson(json['showtime']);
    }
    if (json['tickets'] != null) {
      tickets = <TicketModel>[];
      json['tickets'].forEach((v) {
        tickets!.add(new TicketModel.fromJson(v));
      });
    }
    theater = json['theater'];
    purchasedDate = json['purchasedDate'];
    validUntil = json['validUntil'];
    total = json['total'];
    qrCodeUrl = json['qrCodeUrl'];
    statusId = json['statusId'];
    statusName = json['statusName'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['transactionId'] = this.transactionId;
    data['showtime'] = this.showtime;
    data['theater'] = this.theater;
    data['purchasedDate'] = this.purchasedDate;
    data['validUntil'] = this.validUntil;
    data['total'] = this.total;
    data['qrCodeUrl'] = this.qrCodeUrl;
    data['statusId'] = this.statusId;
    data['statusName'] = this.statusName;
    data['tickets'] = this.tickets!.map((v) => v.toJson()).toList();
    return data;
  }
}

import 'package:term7moviemobile/models/ticket_type_model.dart';

class ShowtimeTicketTypeModel {
  String? id;
  double? receivePrice;
  TicketTypeModel? ticketType;

  ShowtimeTicketTypeModel(
      {this.id,
        this.receivePrice,
        this.ticketType,
      });

  ShowtimeTicketTypeModel.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    receivePrice = json['receivePrice'];
    if (json['ticketType'] != null) {
      TicketTypeModel.fromJson(json['ticketType']);
    }
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['id'] = this.id;
    data['receivePrice'] = this.receivePrice;
    if (this.ticketType != null) {
      data['ticketType'] = this.ticketType!.toJson();
    }
    return data;
  }
}
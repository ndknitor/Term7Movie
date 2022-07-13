import 'package:term7moviemobile/models/seat_model.dart';

class RoomModel {
  int? id;
  int? theaterId;
  int? no;
  int? numberOfColumn;
  int? numberOfRow;
  bool? status;
  List<SeatModel>? seatList;

  RoomModel(
      {this.id,
        this.theaterId,
        this.no,
        this.numberOfColumn, this.numberOfRow, this.status, this.seatList});

  RoomModel.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    theaterId = json['theaterId'];
    no = json['no'];
    numberOfColumn = json['numberOfColumn'];
    numberOfRow = json['numberOfRow'];
    status = json['status'];
    if (json['seatDtos'] != null) {
      seatList = <SeatModel>[];
      json['seatDtos'].forEach((v) {
        seatList!.add(new SeatModel.fromJson(v));
      });
    }
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['id'] = this.id;
    data['theaterId'] = this.theaterId;
    data['no'] = this.no;
    data['numberOfColumn'] = this.numberOfColumn;
    data['numberOfRow'] = this.numberOfRow;
    data['status'] = this.status;
    if (this.seatList != null) {
      data['seatDtos'] = this.seatList!.map((v) => v.toJson()).toList();
    }
    return data;
  }
}
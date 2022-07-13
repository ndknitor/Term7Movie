class SeatModel {
  int? columnPos;
  int? id;
  String? name;
  int? roomId;
  int? rowPos;

  SeatModel({this.columnPos, this.id, this.name, this.roomId, this.rowPos});

  SeatModel.fromJson(Map<String, dynamic> json) {
    columnPos = json['columnPos'];
    id = json['id'] ?? json['seatId'];
    name = json['name'];
    roomId = json['roomId'];
    rowPos = json['rowPos'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['columnPos'] = this.columnPos;
    data['id'] = this.id;
    data['name'] = this.name;
    data['roomId'] = this.roomId;
    data['rowPos'] = this.rowPos;
    return data;
  }
}
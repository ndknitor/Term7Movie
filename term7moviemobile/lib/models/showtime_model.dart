import 'package:term7moviemobile/models/movie_model.dart';
import 'package:term7moviemobile/models/room_model.dart';
import 'package:term7moviemobile/models/showtime_ticket_type_model.dart';

class ShowtimeModel {
  int? id;
  int? theaterId;
  String? theaterName;
  MovieModel? movie;
  String? startTime;
  RoomModel? room;
  List<ShowtimeTicketTypeModel>? showtimeTicketTypes;

  ShowtimeModel(
      {this.id,
      this.theaterId,
      this.movie,
      this.startTime,
      this.showtimeTicketTypes,
      this.theaterName,
      this.room});

  ShowtimeModel.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    theaterId = json['theaterId'];
    if (json['movie'] != null) {
      movie = MovieModel.fromJson(json['movie']);
    }
    startTime = json['startTime'];
    theaterName = json['theaterName'];
    if (json['room'] != null) {
      room = RoomModel.fromJson(json['room']);
    }
    if (json['showtimeTicketTypes'] != null) {
      showtimeTicketTypes = <ShowtimeTicketTypeModel>[];
      json['showtimeTicketTypes'].forEach((v) {
        showtimeTicketTypes!.add(new ShowtimeTicketTypeModel.fromJson(v));
      });
    }
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['id'] = this.id;
    data['theaterId'] = this.theaterId;
    if (this.movie != null) {
      data['movie'] = this.movie!.toJson();
    }
    if (this.room != null) {
      data['room'] = this.room!.toJson();
    }
    data['startTime'] = this.startTime;
    if (this.showtimeTicketTypes != null) {
      data['showtimeTicketTypes'] =
          this.showtimeTicketTypes!.map((v) => v.toJson()).toList();
    }
    data['theaterName'] = this.theaterName;
    return data;
  }
}

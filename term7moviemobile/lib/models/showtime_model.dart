import 'package:term7moviemobile/models/movie_model.dart';
import 'package:term7moviemobile/models/showtime_ticket_type_model.dart';

class ShowtimeModel {
  int? id;
  int? theaterId;
  MovieModel? movie;
  String? startTime;
  // List<ShowtimeTicketTypeModel>? showtimeTicketTypes;

  ShowtimeModel(
      {this.id,
        this.theaterId,
        this.movie,
        this.startTime,});

  ShowtimeModel.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    theaterId = json['theaterId'];
    if (json['movie'] != null) {
      movie = MovieModel.fromJson(json['movie']);
    }
    startTime = json['startTime'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['id'] = this.id;
    data['theaterId'] = this.theaterId;
    if (this.movie != null) {
      data['movie'] = this.movie!.toJson();
    }
    data['startTime'] = this.startTime;
    return data;
  }
}
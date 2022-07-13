import 'package:term7moviemobile/models/showtime_model.dart';
import 'package:term7moviemobile/services/api.dart';

class ShowtimeServices {
  static Future<List<ShowtimeModel>> getShowtimes(Map<String, dynamic>? queryParams) async {
    print(queryParams);
    var res = await Api().api.get('/showtimes', queryParameters: queryParams);
    return List<ShowtimeModel>.from(res.data['showtimes']['results'].map((x) => ShowtimeModel.fromJson(x)));
  }

  static Future<ShowtimeModel> getShowtimeDetail(String? id) async {
    var res = await Api().api.get('/showtimes/$id');
    // print(res.data['showtime']);
    return ShowtimeModel.fromJson(res.data['showtime']);
  }
}
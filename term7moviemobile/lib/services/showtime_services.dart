import 'package:term7moviemobile/models/showtime_model.dart';
import 'package:term7moviemobile/services/api.dart';

class ShowtimeServices {
  static Future<List<ShowtimeModel>> getShowtimes(Map<String, dynamic>? queryParams) async {
    var res = await Api().api.get('/showtimes', queryParameters: queryParams);
    return List<ShowtimeModel>.from(res.data['showtimes']['results'].map((x) => ShowtimeModel.fromJson(x)));
  }
}
import 'package:term7moviemobile/services/api.dart';
import 'package:dio/dio.dart';

class RoomServices {
  static Future<Response> getRoomById(String? id) async {
    return await Api().api.get('/rooms/$id');
  }
}

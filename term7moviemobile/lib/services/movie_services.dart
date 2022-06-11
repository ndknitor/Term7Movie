import 'package:dio/dio.dart';
import 'package:term7moviemobile/models/movie_model.dart';
import 'package:term7moviemobile/services/api.dart';

class MovieServices {
  static Future<List<MovieModel>> getMovieList(Map<String, dynamic>? queryParams) async {
    var res = await Api().api.get('/movies', queryParameters: queryParams);
    return List<MovieModel>.from(res.data['movieList'].map((x) => MovieModel.fromJson(x)));
  }
}
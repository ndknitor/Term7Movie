import 'package:dio/dio.dart';
import 'package:term7moviemobile/models/home_showtime_model.dart';
import 'package:term7moviemobile/models/movie_model.dart';
import 'package:term7moviemobile/services/api.dart';

class MovieServices {
  static Future<List<MovieModel>> getLatestMovies(Map<String, dynamic>? queryParams) async {
    var res = await Api().api.get('/movies', queryParameters: queryParams);
    return List<MovieModel>.from(res.data['movieList']['results'].map((x) => MovieModel.fromJson(x)));
  }

  static Future<List<MovieModel>> getUpcomingMovies(Map<String, dynamic>? queryParams) async {
    var res = await Api().api.get('/movies', queryParameters: queryParams);
    return List<MovieModel>.from(res.data['losslessMovieList'].map((x) => MovieModel.fromJson(x)));
  }

  static Future<List<HomeShowTimeModel>> getMovieShowTimeForHome(Map<String, dynamic>? queryParams) async {
    var res = await Api().api.get('/movies', queryParameters: queryParams);
    return List<HomeShowTimeModel>.from(res.data['movieHomePages'].map((x) => HomeShowTimeModel.fromJson(x)));
  }

  static Future<MovieModel> getMovieDetail(Map<String, dynamic>? queryParams) async {
    var res = await Api().api.get('/movies', queryParameters: queryParams);
    return MovieModel.fromJson(res.data['result']);
  }
}
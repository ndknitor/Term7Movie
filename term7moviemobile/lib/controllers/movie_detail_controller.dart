import 'package:get/get.dart';
import 'package:term7moviemobile/models/movie_model.dart';
import 'package:term7moviemobile/services/movie_services.dart';

class MovieDetailController extends GetxController {
  static MovieDetailController instance = Get.find();
  var isLoading = false.obs;
  MovieModel? movie;
  String id = '';

  void fetchMovieDetail() async {
    try {
      setIsLoading(true);
      id = Get.parameters['id']!;
      movie = await MovieServices.getMovieDetail(
          {'Action': 'detail', 'MovieId': id});
    } finally {
      setIsLoading(false);
    }
  }

  setIsLoading(bool val) {
    isLoading.value = val;
  }
}

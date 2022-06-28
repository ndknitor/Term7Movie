import 'package:get/get.dart';
import 'package:term7moviemobile/models/movie_model.dart';
import 'package:term7moviemobile/services/movie_services.dart';

class MovieDetailController extends GetxController {
  static MovieDetailController instance = Get.find();
  String id = Get.parameters['id']!;
  var isLoading = false.obs;
  MovieModel? movie;

  void fetchMovieDetail() async {
    setIsLoading(false);
    try {
      setIsLoading(true);

      movie = await MovieServices.getMovieDetail(
          {'Action': 'detail', 'MovieId': id});
      update();
    } finally {
      setIsLoading(false);
    }
  }

  setIsLoading(bool val) {
    isLoading.value = val;
  }
}

import 'package:get/get.dart';
import 'package:term7moviemobile/models/movie_model.dart';
import 'package:term7moviemobile/services/movie_services.dart';


class HomeController extends GetxController {
  var isLoading = false;
  List<MovieModel> movies = [];

  @override
  void onInit() {
    fetchMovies();
    super.onInit();
  }

  void fetchMovies() async {
    try{
      isLoading = true;
      update();

      List<MovieModel> _data = await MovieServices.getMovieList({'Action': 'latest'}) as List<MovieModel>;
      movies.assignAll(_data);
      print(movies);
    }finally{
      isLoading = false;
      update();
      print('data fetch done');
    }
  }
}
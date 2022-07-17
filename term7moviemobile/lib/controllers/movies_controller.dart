import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:term7moviemobile/models/movie_model.dart';
import 'package:term7moviemobile/services/movie_services.dart';

class MoviesController extends GetxController with GetSingleTickerProviderStateMixin {
  static MoviesController instance = Get.find();
  late TabController tabController;
  late List<Tab> tabs;
  var isLoading = false.obs;
  List<MovieModel> movies = [];
  var page = 1;
  final tabList = ["Now showing", "Upcoming"];
  List<String> types = [
    "latest",
    "incoming",
  ];
  var isAddLoading = false.obs;
  String searchText = '';

  @override
  void onInit() {
    tabController = TabController(
        length: tabList.length, vsync: this, initialIndex: 0, animationDuration: const Duration(milliseconds: 300));
    tabController.animateTo(0);
    tabs = tabList.map((e) => Tab(text: e)).toList();
    super.onInit();
  }

  @override
  void dispose() {
    pageController.dispose();
    tabController.dispose();
    super.dispose();
  }

  final PageController pageController = PageController(keepPage: true, initialPage: 0);

  updatePage(int index) async {
    pageController.animateToPage(index, duration: const Duration(milliseconds: 300), curve: Curves.easeIn);
    tabController.animateTo(index, duration: const Duration(milliseconds: 300), curve: Curves.easeIn);
    // movies.clear();
    fetchMovies(types[index]);
  }

  void fetchMovies(String type) async {
    try{
      isLoading.value = true;
      List<MovieModel> _data;
      if (type == 'latest') {
        _data = await MovieServices.getLatestMovies({'Action': 'latest', 'PageIndex': page});
      } else {
        _data = await MovieServices.getUpcomingMovies({'Action': 'incoming'});
      }
      movies.assignAll(_data);
    } finally{
      isLoading.value = false;
    }
  }

  void addMovies() async {
    try{
      isAddLoading.value = true;
      page++;
      List<MovieModel> _data;
      if (searchText.isNotEmpty) {
        _data = await MovieServices.getLatestMovies({'Action': 'latest', 'PageIndex': page, 'SearchKey': searchText});
      } else {
        _data = await MovieServices.getLatestMovies({'Action': 'latest', 'PageIndex': page});
      }

      movies.addAll(_data);
    }
    finally{
      isAddLoading(false);
    }
  }

  void searchMovie(String value) async {
    try {
      isLoading.value = true;
      searchText = value;
      List<MovieModel> _data = await MovieServices.getLatestMovies({'Action': 'latest', 'SearchKey': value});
      movies.assignAll(_data);
    } finally {
      isLoading.value = false;
    }
  }
}
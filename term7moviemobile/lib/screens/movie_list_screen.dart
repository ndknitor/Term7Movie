import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:get/get.dart';
import 'package:loading_overlay/loading_overlay.dart';
import 'package:term7moviemobile/controllers/movies_controller.dart';
import 'package:term7moviemobile/utils/theme.dart';
import 'package:term7moviemobile/widgets/card/movie_loading_item.dart';

class MovieListScreen extends StatefulWidget {
  const MovieListScreen({Key? key}) : super(key: key);

  @override
  State<MovieListScreen> createState() => _MovieListScreenState();
}

class _MovieListScreenState extends State<MovieListScreen>
    with SingleTickerProviderStateMixin {
  MoviesController moviesController = Get.put(MoviesController());
  ScrollController scrollController = ScrollController();

  @override
  void initState() {
    super.initState();
    moviesController.fetchMovies('latest');
    scrollController.addListener(() {
      if (scrollController.position.maxScrollExtent ==
          scrollController.position.pixels) {
        moviesController.addMovies();
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      resizeToAvoidBottomInset: false,
      body: Obx(
        () => LoadingOverlay(
          isLoading: moviesController.isLoading.value,
          color: MyTheme.backgroundColor,
          progressIndicator: const CircularProgressIndicator(
            valueColor: AlwaysStoppedAnimation(MyTheme.primaryColor),
          ),
          opacity: 1,
          child: SafeArea(
            child: Container(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.center,
                children: [
                  Container(
                    margin: EdgeInsets.all(16),
                    child: TextFormField(
                      textInputAction: TextInputAction.search,
                      onFieldSubmitted: (value) {
                        moviesController.searchMovie(value);
                      },
                      decoration: const InputDecoration(
                        labelText: 'Search Movie',
                        suffixIcon: Icon(Icons.search),
                        border: OutlineInputBorder(),
                        enabledBorder: const OutlineInputBorder(
                          // width: 0.0 produces a thin "hairline" border
                          borderSide: const BorderSide(color: Colors.grey, width: 0.0),
                        ),
                      ),
                    ),
                  ),
                  TabBar(
                    tabs: moviesController.tabs,
                    controller: moviesController.tabController,
                    indicator: const UnderlineTabIndicator(
                      borderSide: BorderSide(
                        color: MyTheme.primaryColor,
                        width: 2,
                      ),
                      insets: EdgeInsets.all(15),
                    ),
                    indicatorSize: TabBarIndicatorSize.label,
                    indicatorWeight: 6,
                    labelStyle: TextStyle(fontWeight: FontWeight.w600),
                    unselectedLabelColor: MyTheme.textColor,
                    labelColor: MyTheme.primaryColor,
                    isScrollable: false,
                    enableFeedback: false,
                    unselectedLabelStyle:
                    TextStyle(fontWeight: FontWeight.w400),
                    onTap: (index) => moviesController.updatePage(index),
                  ),
                  Expanded(
                    child: PageView.builder(
                      physics: moviesController.page == 2 ? NeverScrollableScrollPhysics() : AlwaysScrollableScrollPhysics(),
                      controller: moviesController.pageController,
                      itemCount: moviesController.tabs.length,
                      itemBuilder: (_, index) {
                        return LayoutBuilder(
                          builder: (context, constraint) {
                            return Obx(() => GridView.builder(
                                  controller: scrollController,
                                  shrinkWrap: true,
                                  gridDelegate:
                                      SliverGridDelegateWithFixedCrossAxisCount(
                                    crossAxisCount:
                                        constraint.maxWidth > 480 ? 4 : 2,
                                    childAspectRatio: 0.8,
                                  ),
                                  itemBuilder: (_, index) {
                                    if (moviesController.movies.length <=
                                        index) {
                                      return MovieLoadingItem();
                                    }
                                    return Padding(
                                      padding: const EdgeInsets.only(
                                          top: 10, left: 20.0, right: 10),
                                      child: GestureDetector(
                                        onTap: () {
                                          Get.toNamed(
                                              "/detail/${moviesController.movies[index].movieId}");
                                        },
                                        child: SingleChildScrollView(
                                          physics:
                                              const NeverScrollableScrollPhysics(),
                                          child: Column(
                                            crossAxisAlignment:
                                                CrossAxisAlignment.start,
                                            children: [
                                              ClipRRect(
                                                borderRadius:
                                                    BorderRadius.circular(10),
                                                child: Image.network(
                                                  moviesController.movies[index]
                                                              .posterImgUrl
                                                              ?.toString()
                                                              .length ==
                                                          0
                                                      ? 'https://westsiderc.org/wp-content/uploads/2019/08/Image-Not-Available.png'
                                                      : moviesController
                                                          .movies[index]
                                                          .posterImgUrl!,
                                                  height: 180,
                                                  width: 150,
                                                  fit: BoxFit.cover,
                                                ),
                                              ),
                                              const SizedBox(
                                                height: 8,
                                              ),
                                              Container(
                                                width: 150,
                                                child: Text(
                                                  moviesController.movies[index]
                                                          .title ??
                                                      '',
                                                  maxLines: 1,
                                                  overflow:
                                                      TextOverflow.ellipsis,
                                                  softWrap: true,
                                                  style: TextStyle(
                                                      fontSize: 14,
                                                      color: Colors.black
                                                          .withOpacity(0.8),
                                                      fontWeight:
                                                          FontWeight.w500),
                                                ),
                                              ),
                                            ],
                                          ),
                                        ),
                                      ),
                                    );
                                  },
                                  itemCount: moviesController.isAddLoading.value
                                      ? moviesController.movies.length + 10
                                      : moviesController.movies.length + 2,
                                ));
                          },
                        );
                      },
                    ),
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}

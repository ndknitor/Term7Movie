import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:loading_overlay/loading_overlay.dart';
import 'package:term7moviemobile/controllers/movie_detail_controller.dart';
import 'package:term7moviemobile/utils/constants.dart';
import 'package:term7moviemobile/utils/convert_color.dart';
import 'package:term7moviemobile/utils/theme.dart';
import 'package:term7moviemobile/widgets/arrow_back.dart';
import 'package:term7moviemobile/widgets/movie_detail/company_list.dart';
import 'package:term7moviemobile/widgets/date_picker.dart';
import 'package:term7moviemobile/widgets/movie_detail/background_widget.dart';
import 'package:term7moviemobile/widgets/movie_detail/cast_bar.dart';
import 'package:term7moviemobile/widgets/movie_detail/trailer_bar.dart';

class MovieDetailScreen extends StatefulWidget {
  const MovieDetailScreen({Key? key}) : super(key: key);

  @override
  State<MovieDetailScreen> createState() => _MovieDetailScreenState();
}

class _MovieDetailScreenState extends State<MovieDetailScreen>
    with SingleTickerProviderStateMixin {
  late TabController _tabController;
  MovieDetailController controller = Get.put(MovieDetailController());

  @override
  void initState() {
    super.initState();
    controller.fetchMovieDetail();
    _tabController = TabController(length: 2, vsync: this);
  }

  @override
  Widget build(BuildContext context) {
    final Size size = MediaQuery.of(context).size;
    return Scaffold(body: LayoutBuilder(
      builder: (ctx, constraints) {
        return Obx(
          () => LoadingOverlay(
            isLoading: controller.isLoading.value,
            color: MyTheme.backgroundColor,
            progressIndicator: const CircularProgressIndicator(
              valueColor: AlwaysStoppedAnimation(MyTheme.primaryColor),
            ),
            opacity: 1,
            child: SingleChildScrollView(
              child: controller.movie == null
                  ? Container(
                      width: size.width,
                      height: size.height,
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: [
                          Text(
                            "Can't find the movie",
                            style: TextStyle(
                                fontWeight: FontWeight.w500, fontSize: 18),
                          ),
                          ElevatedButton.icon(
                            style: ElevatedButton.styleFrom(
                                primary: MyTheme.primaryColor),
                            icon: Icon(Icons.arrow_back_rounded, size: 18),
                            label: Text('Go Back'),
                            onPressed: () {
                              Get.back();
                            },
                          )
                        ],
                      ),
                    )
                  : Stack(
                      children: [
                        BackgroundWidget(
                          size: size,
                          coverImgUrl: controller.movie?.coverImgUrl ??
                              Constants.defaultImage,
                        ),
                        Container(
                          height: size.height / 3,
                          decoration: BoxDecoration(
                              gradient: LinearGradient(
                                  colors: [
                                MyTheme.primaryColor.withOpacity(0.3),
                                MyTheme.primaryColor.withOpacity(0.1)
                              ],
                                  begin: Alignment.topCenter,
                                  end: Alignment.bottomCenter)),
                        ),
                        const ArrowBack(color: Colors.white,),
                        Column(
                          mainAxisAlignment: MainAxisAlignment.end,
                          children: [
                            Padding(
                              padding: EdgeInsets.only(
                                  left: 16, top: size.height / 4),
                              child: Row(
                                children: [
                                  SizedBox(
                                    width: size.width / 2.5,
                                    child: ClipRRect(
                                      borderRadius: BorderRadius.circular(10),
                                      child: Image.network(
                                        controller.movie?.posterImgUrl ??
                                            'https://westsiderc.org/wp-content/uploads/2019/08/Image-Not-Available.png',
                                        fit: BoxFit.cover,
                                      ),
                                    ),
                                  ),
                                  Expanded(
                                      child: Column(
                                    children: [
                                      Container(
                                        padding: EdgeInsets.only(
                                            left: 8,
                                            bottom: 8,
                                            top: size.height / 10),
                                        width: size.width,
                                        child: Text(
                                          controller.movie?.title ?? '',
                                          style: TextStyle(
                                              fontWeight: FontWeight.w600,
                                              fontSize: 20,
                                              height: 1.2,
                                              decoration: TextDecoration.none,
                                              color: MyTheme.textColor),
                                        ),
                                      ),
                                      SingleChildScrollView(
                                        scrollDirection: Axis.horizontal,
                                        // physics: NeverScrollableScrollPhysics(),
                                        child: Container(
                                          padding: const EdgeInsets.only(
                                              left: 8, bottom: 8),
                                          width: size.width,
                                          child: Row(
                                            mainAxisSize: MainAxisSize.min,
                                            children: controller
                                                .movie!.categories!
                                                .map((e) =>
                                                    Builder(builder: (context) {
                                                      return Container(
                                                        margin: EdgeInsets.only(
                                                            right: 8),
                                                        alignment:
                                                            Alignment.center,
                                                        decoration: BoxDecoration(
                                                          borderRadius:
                                                              BorderRadius
                                                                  .circular(4),
                                                          color: HexColor(
                                                                  e.color!)
                                                              .withOpacity(0.2),
                                                        ),
                                                        padding:
                                                            EdgeInsets.symmetric(
                                                                vertical: 2,
                                                                horizontal: 6),
                                                        child: Text(
                                                          e.name!,
                                                          style: TextStyle(
                                                            fontWeight:
                                                                FontWeight.w600,
                                                            fontSize: 10,
                                                            color: HexColor(
                                                                e.color!),
                                                            decoration:
                                                                TextDecoration
                                                                    .none,
                                                          ),
                                                        ),
                                                      );
                                                    }))
                                                .toList(),
                                          ),
                                        ),
                                      ),
                                      Padding(
                                        padding: const EdgeInsets.only(
                                            left: 8, bottom: 8),
                                        child: Row(
                                          children: [
                                            Container(
                                              margin: EdgeInsets.only(right: 8),
                                              width: 30,
                                              alignment: Alignment.center,
                                              decoration: BoxDecoration(
                                                  borderRadius:
                                                      BorderRadius.circular(4),
                                                  color: controller.movie?.ageRestrict ==
                                                      0 ? MyTheme.successColor : MyTheme.errorColor),
                                              padding: EdgeInsets.symmetric(
                                                  vertical: 2, horizontal: 6),
                                              child: Text(
                                                controller.movie?.ageRestrict ==
                                                        0
                                                    ? 'P'
                                                    : 'C' + controller.movie!.ageRestrict.toString(),
                                                style: TextStyle(
                                                  fontWeight: FontWeight.w500,
                                                  fontSize: 10,
                                                  color: Colors.white,
                                                  decoration:
                                                      TextDecoration.none,
                                                ),
                                              ),
                                            ),
                                            Row(
                                              children: [
                                                Icon(Icons.access_time,
                                                    color: MyTheme.primaryColor,
                                                    size: 18),
                                                SizedBox(
                                                  width: 2,
                                                ),
                                                Text(
                                                  controller.movie!.duration
                                                          .toString() +
                                                      ' min',
                                                  style: TextStyle(
                                                    fontWeight: FontWeight.w400,
                                                    fontSize: 10,
                                                    color: MyTheme.textColor,
                                                  ),
                                                ),
                                              ],
                                            ),
                                            SizedBox(
                                              width: 8,
                                            ),
                                            Row(
                                              children: [
                                                Icon(Icons.date_range,
                                                    color: MyTheme.primaryColor,
                                                    size: 18),
                                                SizedBox(
                                                  width: 2,
                                                ),
                                                Text(
                                                  controller.movie!.releaseDate
                                                      .toString()
                                                      .split('T')
                                                      .first,
                                                  style: TextStyle(
                                                    fontWeight: FontWeight.w400,
                                                    fontSize: 10,
                                                    color: MyTheme.textColor,
                                                  ),
                                                ),
                                              ],
                                            ),
                                          ],
                                        ),
                                      ),
                                      Padding(
                                        padding: const EdgeInsets.only(
                                            left: 8, bottom: 8),
                                        child: Row(
                                          children: [
                                            Icon(Icons.language,
                                                color: MyTheme.primaryColor,
                                                size: 22),
                                            SizedBox(
                                              width: 4,
                                            ),
                                            Text(
                                              'Languages:',
                                              style: TextStyle(
                                                fontWeight: FontWeight.w300,
                                                fontSize: 12,
                                                color: MyTheme.textColor,
                                              ),
                                            ),
                                          ],
                                        ),
                                      )
                                    ],
                                  ))
                                ],
                              ),
                            ),
                            SizedBox(
                              height: size.height + 200,
                              child: Column(
                                children: [
                                  Container(
                                    margin: const EdgeInsets.only(top: 4),
                                    alignment: Alignment.center,
                                    width: size.width,
                                    child: TabBar(
                                      tabs: const [
                                        Tab(text: 'About Movie'),
                                        Tab(text: 'Showtime'),
                                      ],
                                      controller: _tabController,
                                      indicatorSize: TabBarIndicatorSize.label,
                                      labelColor: MyTheme.primaryColor,
                                      unselectedLabelColor: MyTheme.grayColor,
                                      labelStyle: TextStyle(
                                        fontWeight: FontWeight.w600,
                                        fontSize: 16,
                                      ),
                                      unselectedLabelStyle: TextStyle(
                                        fontWeight: FontWeight.w500,
                                        fontSize: 16,
                                      ),
                                      indicatorColor: MyTheme.primaryColor,
                                    ),
                                  ),
                                  Expanded(
                                    child: TabBarView(
                                      controller: _tabController,
                                      children: [
                                        Column(
                                          crossAxisAlignment:
                                              CrossAxisAlignment.stretch,
                                          children: [
                                            buildTitle('Description'),
                                            Padding(
                                              padding:
                                                  EdgeInsets.only(left: 24),
                                              child: Text(
                                                controller.movie?.description ??
                                                    'There is no description',
                                                style: TextStyle(
                                                    fontWeight: FontWeight.w400,
                                                    fontSize: 14,
                                                    color: Colors.black54,
                                                    decoration:
                                                        TextDecoration.none),
                                              ),
                                            ),
                                            buildTitle('Actors'),
                                            CastBar(size: size),
                                            buildTitle('Director'),
                                            CastBar(size: size),
                                            buildTitle('Trailer'),
                                            // const TrailerBar()
                                          ],
                                        ),
                                        Column(
                                            children: [
                                              MyDatePicker(),
                                              CompanyList(),
                                            ],
                                          ),
                                      ],
                                    ),
                                  ),
                                ],
                              ),
                            )
                          ],
                        ),
                      ],
                    ),
            ),
          ),
        );
      },
    ));
  }

  Padding buildTitle(String content) {
    return Padding(
      padding: const EdgeInsets.all(16),
      child: Text(
        content,
        style: TextStyle(
            fontWeight: FontWeight.w500,
            fontSize: 22,
            decoration: TextDecoration.none,
            color: MyTheme.textColor),
      ),
    );
  }
}

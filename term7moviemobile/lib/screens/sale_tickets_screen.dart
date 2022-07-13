import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';
import 'package:loading_overlay/loading_overlay.dart';
import 'package:term7moviemobile/controllers/sale_tickets_controller.dart';
import 'package:term7moviemobile/models/ticket_model.dart';
import 'package:term7moviemobile/utils/theme.dart';

class SaleTicketsScreen extends StatefulWidget {
  const SaleTicketsScreen({Key? key}) : super(key: key);

  @override
  State<SaleTicketsScreen> createState() => _SaleTicketsScreenState();
}

class _SaleTicketsScreenState extends State<SaleTicketsScreen> {
  ScrollController scrollController = ScrollController();
  SaleTicketsController controller = Get.put(SaleTicketsController());

  @override
  void initState() {
    super.initState();
    controller.fetchData();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: PreferredSize(
        preferredSize: const Size.fromHeight(60),
        child: AppBar(
          title: Padding(
            padding: const EdgeInsets.only(left: 12, top: 36),
            child: Text(
              "Flash sale".toUpperCase(),
              style: TextStyle(
                  color: MyTheme.primaryColor, fontWeight: FontWeight.w700),
            ),
          ),
          backgroundColor: Colors.transparent,
          elevation: 0,
        ),
      ),
      body: Obx(() => LoadingOverlay(
            isLoading: controller.isLoading.value,
            color: MyTheme.backgroundColor,
            opacity: 1,
            progressIndicator: const CircularProgressIndicator(
              valueColor: AlwaysStoppedAnimation(MyTheme.primaryColor),
            ),
            child: SafeArea(
              child: Container(
                // padding: EdgeInsets.only(top: 40),
                child: LayoutBuilder(
                  builder: (context, constraint) {
                    return GridView.builder(
                      controller: scrollController,
                      shrinkWrap: true,
                      gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
                        crossAxisCount: constraint.maxWidth > 480 ? 2 : 2,
                        mainAxisExtent: 280,
                        childAspectRatio: 0.8,
                      ),
                      itemBuilder: (_, index) {
                        if (controller.tickets.length <= index) {
                          return Container();
                        }
                        return Container(
                          margin: const EdgeInsets.only(left: 16.0, right: 10),
                          alignment: Alignment.center,
                          child: GestureDetector(
                            onTap: () {
                              Get.toNamed("/checkout", arguments: {
                                'showtime': controller.tickets[index].showtime,
                                'tickets':  controller.tickets.take(index).toList(),
                                'total': controller.tickets[index].sellingPrice
                              });
                            },
                            child: Container(
                              height: 240,
                              // color: Colors.cyanAccent,
                              child: Stack(
                                children: [
                                  Positioned(
                                    bottom: 0,
                                    left: 0,
                                    right: 0,
                                    child: Container(
                                      height: 200,
                                      decoration: BoxDecoration(
                                        borderRadius: BorderRadius.circular(8),
                                        color: MyTheme.borderColor,
                                      ),
                                    ),
                                  ),
                                  Positioned(
                                    left: -11,
                                    right: -11,
                                    bottom: 36,
                                    child: Row(
                                      mainAxisAlignment:
                                          MainAxisAlignment.spaceBetween,
                                      crossAxisAlignment:
                                          CrossAxisAlignment.center,
                                      children: [
                                        CircleAvatar(
                                          radius: 14,
                                          backgroundColor: Colors.white,
                                        ),
                                        Expanded(
                                          child: Text(
                                            "---------------",
                                            style: TextStyle(
                                                color: MyTheme.grayColor),
                                          ),
                                        ),
                                        CircleAvatar(
                                          radius: 14,
                                          backgroundColor: Colors.white,
                                        )
                                      ],
                                    ),
                                  ),
                                  Positioned(
                                    top: 0,
                                    left: 0,
                                    right: 0,
                                    child: Column(
                                      mainAxisAlignment:
                                          MainAxisAlignment.center,
                                      children: [
                                        ClipRRect(
                                          borderRadius:
                                              BorderRadius.circular(10),
                                          child: Image.network(
                                            // controller.tickets[index].showtime!.movie?.posterImgUrl?.length == 0 ?
                                            'https://westsiderc.org/wp-content/uploads/2019/08/Image-Not-Available.png',
                                            // : controller.tickets[index].showtime?.movie?.posterImgUrl,
                                            height: 100,
                                            width: 100,
                                            fit: BoxFit.cover,
                                          ),
                                        ),
                                        const SizedBox(
                                          height: 8,
                                        ),
                                        Container(
                                            width: 130,
                                            child: Row(
                                              mainAxisAlignment:
                                                  MainAxisAlignment.center,
                                              children: [
                                                // Icon(Icons.access_time, color: MyTheme.warningColor, size: 16,),
                                                // const SizedBox(
                                                //   width: 4,
                                                // ),
                                                Text(
                                                  DateFormat.Hm().format(DateTime
                                                              .parse(controller
                                                                  .tickets[
                                                                      index]
                                                                  .showtime!
                                                                  .startTime!)
                                                          .add(Duration(
                                                              hours: 7))) +
                                                      ' - ' +
                                                      DateFormat.yMMMd()
                                                          .format(DateTime
                                                              .parse(controller
                                                                  .tickets[
                                                                      index]
                                                                  .showtime!
                                                                  .startTime!))
                                                          .toString(),
                                                  textAlign: TextAlign.center,
                                                  style: TextStyle(
                                                      fontSize: 12,
                                                      color:
                                                          MyTheme.warningColor,
                                                      fontWeight:
                                                          FontWeight.w500),
                                                ),
                                              ],
                                            )),
                                        const SizedBox(
                                          height: 4,
                                        ),
                                        Container(
                                          width: 130,
                                          child: Text(
                                            // moviesController.movies[index]
                                            //     .title ??
                                            'movie titlesádgshdfhfdhdhbsdb  đfsbdb gdbdb h',
                                            maxLines: 2,
                                            overflow: TextOverflow.ellipsis,
                                            softWrap: true,
                                            textAlign: TextAlign.center,
                                            style: TextStyle(
                                                fontSize: 16,
                                                color: Colors.black
                                                    .withOpacity(0.8),
                                                fontWeight: FontWeight.w600),
                                          ),
                                        ),
                                        const SizedBox(
                                          height: 30,
                                        ),
                                        Container(
                                            width: 130,
                                            child: Row(
                                              mainAxisAlignment:
                                                  MainAxisAlignment.center,
                                              children: [
                                                Icon(
                                                  Icons
                                                      .confirmation_num_outlined,
                                                  color: MyTheme.grayColor,
                                                  size: 18,
                                                ),
                                                Text(
                                                  controller.tickets[index].seat!.name! + ' - ',
                                                  textAlign: TextAlign.center,
                                                  style: TextStyle(
                                                      fontSize: 14,
                                                      color: MyTheme.grayColor,
                                                      fontWeight:
                                                          FontWeight.w500),
                                                ),
                                                const SizedBox(
                                                  width: 2,
                                                ),
                                                Text(
                                                  controller.tickets[index].sellingPrice!.toStringAsFixed(0) + ' VND',
                                                  textAlign: TextAlign.center,
                                                  style: TextStyle(
                                                      fontSize: 14,
                                                      color: MyTheme.grayColor,
                                                      fontWeight:
                                                          FontWeight.w700),
                                                ),
                                              ],
                                            )),
                                        // Container(
                                        //     width: 130,
                                        //     child: Row(
                                        //       mainAxisAlignment: MainAxisAlignment.center,
                                        //       children: [
                                        //         Icon(Icons.price_change, color: MyTheme.grayColor, size: 18,),
                                        //         const SizedBox(
                                        //           width: 2,
                                        //         ),
                                        //         Text(
                                        //           '1000 VND',
                                        //           textAlign: TextAlign.center,
                                        //           style: TextStyle(
                                        //               fontSize: 14,
                                        //               color: MyTheme.grayColor,
                                        //               fontWeight: FontWeight.w600),
                                        //         ),
                                        //       ],
                                        //     )),
                                      ],
                                    ),
                                  ),
                                ],
                              ),
                            ),
                          ),
                        );
                      },
                      itemCount: controller.tickets.length,
                    );
                  },
                ),
              ),
            ),
          )),
    );
  }
}

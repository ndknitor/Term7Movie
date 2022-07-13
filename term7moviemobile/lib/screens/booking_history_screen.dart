import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';
import 'package:loading_overlay/loading_overlay.dart';
import 'package:term7moviemobile/controllers/booking_history_controller.dart';
import 'package:term7moviemobile/utils/theme.dart';
import 'package:term7moviemobile/widgets/arrow_back.dart';
import 'package:term7moviemobile/widgets/card/transaction_loading_item.dart';

class BookingHistoryScreen extends StatefulWidget {
  const BookingHistoryScreen({Key? key}) : super(key: key);

  @override
  State<BookingHistoryScreen> createState() => _BookingHistoryScreenState();
}

class _BookingHistoryScreenState extends State<BookingHistoryScreen> {
  late BookingHistoryController controller;
  ScrollController scrollController = ScrollController();

  @override
  void initState() {
    super.initState();
    Get.delete<BookingHistoryController>();
    controller = Get.put(BookingHistoryController());
    controller.fetchData();
    scrollController.addListener(() {
      if (scrollController.position.maxScrollExtent ==
          scrollController.position.pixels) {
        // controller.addMovies();
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    final Size size = MediaQuery.of(context).size;

    return Scaffold(
        resizeToAvoidBottomInset: false,
        body: Obx(() => LoadingOverlay(
            isLoading: controller.isLoading.value,
            color: MyTheme.backgroundColor,
            opacity: 1,
            progressIndicator: const CircularProgressIndicator(
              valueColor: AlwaysStoppedAnimation(MyTheme.primaryColor),
            ),
            child: SafeArea(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    mainAxisAlignment: MainAxisAlignment.start,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: [
                      ArrowBack(
                        color: MyTheme.bottomBarColor,
                        callback: () {
                          Get.toNamed("/");
                        },
                      ),
                      Expanded(
                        child: Center(
                          child: Padding(
                            padding:
                                const EdgeInsets.only(top: 36.0, right: 24),
                            child: Text("Transaction History",
                                style: TextStyle(
                                    fontWeight: FontWeight.w600, fontSize: 18)),
                          ),
                        ),
                      ),
                    ],
                  ),
                  controller.transactions.length > 0
                      ? Container(
                          height: size.height - 130,
                          padding: EdgeInsets.symmetric(
                              vertical: 24, horizontal: 16),
                          color: MyTheme.borderColor,
                          child: ListView.builder(
                            controller: scrollController,
                            scrollDirection: Axis.vertical,
                            itemCount: controller.transactions.length,
                            shrinkWrap: true,
                            itemBuilder: (_, i) {
                              return GestureDetector(
                                onTap: () {
                                  if (controller.transactions[i].statusName ==
                                      'Successful') {
                                    Get.toNamed(
                                        "/history/${controller.transactions[i].transactionId}");
                                  }
                                },
                                child: Container(
                                  height: 150,
                                  width: size.width,
                                  margin: EdgeInsets.only(top: 12),
                                  decoration: BoxDecoration(
                                    color: Colors.white,
                                    borderRadius: BorderRadius.circular(8),
                                  ),
                                  padding: EdgeInsets.symmetric(
                                      vertical: 8, horizontal: 12),
                                  child: Row(
                                    children: [
                                      ClipRRect(
                                        borderRadius: BorderRadius.circular(10),
                                        child: Image.network(
                                          controller
                                                      .transactions[i]
                                                      .showtime!
                                                      .movie!
                                                      .posterImgUrl!
                                                      .length ==
                                                  0
                                              ? 'https://westsiderc.org/wp-content/uploads/2019/08/Image-Not-Available.png'
                                              : controller
                                                  .transactions[i]
                                                  .showtime!
                                                  .movie!
                                                  .posterImgUrl!,
                                          height: 130,
                                          width: 90,
                                          fit: BoxFit.cover,
                                        ),
                                      ),
                                      const SizedBox(
                                        width: 8,
                                      ),
                                      SizedBox(
                                        height: double.infinity,
                                        child: Column(
                                          crossAxisAlignment:
                                              CrossAxisAlignment.start,
                                          children: [
                                            SizedBox(
                                              width: size.width - 160,
                                              child: Text(
                                                controller.transactions[i]
                                                    .showtime!.movie!.title!,
                                                maxLines: 2,
                                                overflow: TextOverflow.ellipsis,
                                                softWrap: true,
                                                style: TextStyle(
                                                    fontSize: 16,
                                                    color: MyTheme.textColor,
                                                    fontWeight:
                                                        FontWeight.w700),
                                              ),
                                            ),
                                            SizedBox(
                                              height: 2,
                                            ),
                                            Row(
                                              children: [
                                                Text(
                                                  controller.transactions[i]
                                                      .showtime!.theaterName!,
                                                  style:
                                                      TextStyle(fontSize: 12),
                                                ),
                                                // Text(
                                                //   'Room '
                                                //   + controller.transactions[i]
                                                //       .showtime!.room!.no.toString()
                                                //   ,
                                                //   style: TextStyle(
                                                //       fontSize: 12,
                                                //       fontWeight: FontWeight.w600),
                                                // ),
                                              ],
                                            ),
                                            const SizedBox(
                                              height: 4,
                                            ),
                                            Row(
                                              children: [
                                                Text(
                                                  DateFormat.Hm().format(DateTime
                                                              .parse(controller
                                                                  .transactions[
                                                                      i]
                                                                  .showtime!
                                                                  .startTime!)
                                                          .add(Duration(
                                                              hours: 7))) +
                                                      ' - ',
                                                  style: TextStyle(
                                                      fontSize: 12,
                                                      fontWeight:
                                                          FontWeight.w600),
                                                ),
                                                Text(
                                                  DateFormat.yMMMd()
                                                      .format(DateTime.parse(
                                                          controller
                                                              .transactions[i]
                                                              .showtime!
                                                              .startTime!))
                                                      .toString(),
                                                  style:
                                                      TextStyle(fontSize: 12),
                                                ),
                                              ],
                                            ),
                                            const SizedBox(
                                              height: 4,
                                            ),
                                            Container(
                                              margin: EdgeInsets.only(right: 8),
                                              alignment: Alignment.center,
                                              decoration: BoxDecoration(
                                                  borderRadius:
                                                      BorderRadius.circular(4),
                                                  color: controller
                                                              .transactions[i]
                                                              .statusName ==
                                                          'Successful'
                                                      ? MyTheme.successColor
                                                      : controller
                                                                  .transactions[
                                                                      i]
                                                                  .statusName ==
                                                              'Pending'
                                                          ? MyTheme.warningColor
                                                          : MyTheme.errorColor),
                                              padding: EdgeInsets.symmetric(
                                                  vertical: 2, horizontal: 6),
                                              child: Text(
                                                controller.transactions[i]
                                                    .statusName!,
                                                style: TextStyle(
                                                  fontWeight: FontWeight.w500,
                                                  fontSize: 10,
                                                  color: Colors.white,
                                                  decoration:
                                                      TextDecoration.none,
                                                ),
                                              ),
                                            ),
                                          ],
                                        ),
                                      )
                                    ],
                                  ),
                                ),
                              );
                            },
                          ))
                      : Container(
                          height: size.height - 280,
                          margin: EdgeInsets.all(24),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.center,
                            mainAxisAlignment: MainAxisAlignment.center,
                            children: [
                              Image.asset(
                                "assets/images/ticket.png",
                              ),
                              Text(
                                "No Transactions",
                                style: TextStyle(
                                    fontSize: 20,
                                    color: MyTheme.textColor,
                                    fontWeight: FontWeight.w700),
                                textAlign: TextAlign.center,
                              ),
                              Text(
                                "Let's start watching the movie for the first deal.",
                                style: TextStyle(
                                    fontSize: 16, color: MyTheme.grayColor),
                                textAlign: TextAlign.center,
                              ),
                            ],
                          ),
                        )
                ],
              ),
            ))));
  }
}

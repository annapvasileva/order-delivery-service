import {Controller, Get, Post, Render} from '@nestjs/common';

@Controller()
export class AppController {
    @Get()
    @Render('orders')
    getOrders() {
        return {};
    }
    
    @Get('create_order')
    @Render('create_order')
    getCreateOrder() {
        return {};
    }
}

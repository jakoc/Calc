import http from 'k6/http';
import { sleep, check } from 'k6';

export const options = {
    stages: [
        { duration: '10s', target: 50 }, // 50 brugere over 10 sekunder
        { duration: '20s', target: 100 }, // 100 brugere over 20 sekunder
    ]
};

export default function () {
    let res = http.get('https://129.151.223.141/api/calculator/history');
    check(res, {
        'API svarede med 200': (r) => r.status === 200,
        'Svartid under 500ms': (r) => r.timings.duration < 500,
    });
    sleep(1);
}
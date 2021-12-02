using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentMenagement.Infrastructure.Repositories;
using StudentMenagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentMenagement.Controllers
{
    [AllowAnonymous]
    [ApiController]
    //[Route("[Controller]")]
    //[Route("todo")]
    [Route("api/[controller]/[action]")]
    public class TodoController : ControllerBase
    {
        //注入仓储服务，TodoItem的主键Id为long类型，仓储服务参数也需要对应一致
        private readonly IRepository<TodoItem, long> _todoItemRepository;

        public TodoController(IRepository<TodoItem, long> todoRepository)
        {
            this._todoItemRepository = todoRepository;
        }

        /// <summary>
        /// 获取所有待办事项
        /// </summary>
        /// <returns> </returns>
        // GET:api/Todo
        [HttpGet]
        public async Task<ActionResult<List<TodoItem>>> GetTodo()
        {      
            //获取所有的待办事项列表
            var models = await _todoItemRepository.GetAllListAsync();
            return models;
        }

        #region 根据Id获取待办事项

        /// <summary>
        /// 通过Id获取待办事项
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        // GET:api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _todoItemRepository.FirstOrDefaultAsync(a => a.Id == id);

            if (todoItem == null)
            {   //返回404状态码
                return NotFound();
            }

            return todoItem;
        }

        #endregion 根据Id获取待办事项

        #region 更新待办事项

        /// <summary>
        /// 更新待办事项
        /// </summary>
        /// <param name="id"> </param>
        /// <param name="todoItem"> </param>
        /// <returns> </returns>
        // PUT:api/Todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            await _todoItemRepository.UpdateAsync(todoItem);

            //返回状态码204
            return NoContent();
        }

        #endregion 更新待办事项

        #region 添加待办事项

        ///// <summary>
        ///// 添加待办事项
        ///// </summary>
        ///// <param name="todoItem"> </param>
        ///// <returns> </returns>
        /// <summary>
        /// 添加待办事项
        /// </summary>
        /// <param name="todoItem"></param>
        /// <returns></returns>
        // POST:api/Todo
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            await _todoItemRepository.InsertAsync(todoItem);
            //创建一个reatedAtActionResult对象，它生成一个状态码为Status201 
            //Created的响应
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        #endregion 添加待办事项

        #region 删除指定Id的待办事项

        /// <summary>
        /// 删除指定Id的待办事项
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        // DELETE:api/Todo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id)
        {
            var todoItem = await _todoItemRepository.FirstOrDefaultAsync(a => a.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }
            await _todoItemRepository.DeleteAsync(todoItem);
            return todoItem;
        }

        #endregion 删除指定Id的待办事项

    }
}
